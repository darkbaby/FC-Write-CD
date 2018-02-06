using System;
using System.Management;

namespace FCWriteCD
{
    class DiscDeviceWatcher
    {
        private ManagementEventWatcher watcher;

        public delegate void OnEventInsert();
        public delegate void OnEventEject();

        private event OnEventInsert onInserted;
        private event OnEventEject onEjected;

        public static bool isDiscInserted;
        //public bool IsDiscInserted
        //{
        //    get { return IsDiscInserted; }
        //}

        public DiscDeviceWatcher(OnEventInsert onInserted, OnEventEject onEjected)
        {
            isDiscInserted = DetectCDOnDevice();
            this.onInserted += new OnEventInsert(onInserted);
            this.onEjected += new OnEventEject(onEjected);
            StartWatcherOnDevice();
        }

        private bool DetectCDOnDevice()
        {
            SelectQuery query = new SelectQuery("select * from win32_logicaldisk where drivetype=5");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            bool returnValue = false;
            foreach (ManagementObject mo in searcher.Get())
            {
                // If both properties are null I suppose there's no CD
                if ((mo["volumename"] != null) || (mo["volumeserialnumber"] != null) || (mo["Access"] != null))
                {
                    returnValue = true;
                    break;
                }
                else
                {
                    continue;
                }
            }

            return returnValue; 
        }

        private void StartWatcherOnDevice()
        {
            try
            {
                WqlEventQuery q = new WqlEventQuery();
                q.EventClassName = "__InstanceModificationEvent";
                q.WithinInterval = new TimeSpan(0, 0, 1);
                q.Condition = @"TargetInstance ISA 'Win32_LogicalDisk' and TargetInstance.DriveType = 5";

                ConnectionOptions opt = new ConnectionOptions();
                opt.EnablePrivileges = true;
                opt.Authority = null;
                opt.Authentication = AuthenticationLevel.Default;
                ManagementScope scope = new ManagementScope("\\root\\CIMV2", opt);

                watcher = new ManagementEventWatcher(scope, q);
                watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
                watcher.Start();
            }
            catch (ManagementException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject wmiDevice = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            string driveName = (string)wmiDevice["DeviceID"];
            Console.WriteLine(driveName);
            Console.WriteLine(wmiDevice.Properties["VolumeName"].Value);
            Console.WriteLine(wmiDevice.Properties["VolumeSerialNumber"].Value);
            Console.WriteLine(wmiDevice.Properties["Access"].Value);

            //foreach (PropertyData prop in wmiDevice.Properties)
            //{
            //    Console.WriteLine(prop.Name + " : " + prop.Value);
            //}
            if (wmiDevice.Properties["VolumeName"].Value != null || wmiDevice.Properties["VolumeSerialNumber"].Value != null
                    || wmiDevice.Properties["Access"].Value != null)
            {
                Console.WriteLine("CD has been inserted");
                isDiscInserted = true;
                onInserted();
            }
            else
            {
                Console.WriteLine("CD has been ejected");
                isDiscInserted = false;
                onEjected();
            }
        }

        public void EndWatcher()
        {
            watcher.Stop();
            watcher.Dispose();
        }
    }
}
