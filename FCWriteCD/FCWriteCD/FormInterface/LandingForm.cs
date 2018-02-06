using FCWriteCD.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCWriteCD.FormInterface
{
    public partial class LandingForm : Form
    {
        string computerName = Environment.MachineName;
        string pathForSearchAccountMonth;

        public LandingForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
        }

        private int Initialize()
        {
            if (!DAOHelper.Instance.CheckConnection())
            {
                return 1;
            }

            if (!DAOHelper.Instance.GetAccessForWorking(computerName))
            {
                return 2;
            }

            pathForSearchAccountMonth = DAOHelper.Instance.GetSettingValue("targetPathMapDrive");
            if (pathForSearchAccountMonth.EndsWith(@"\"))
            {
                pathForSearchAccountMonth = pathForSearchAccountMonth.Substring(0, pathForSearchAccountMonth.Length - 1);
            }
            string deviceID = DAOHelper.Instance.GetDeviceID(computerName);
            if (deviceID != null)
            {
                WriteCDModule.Instance.Initialize(deviceID);
                int returnCodeFromSetup = SetupBeforeMainForm();
                if (returnCodeFromSetup == 0)
                {
                    Program.isContinueMainForm = true;
                    Program.mainForm = new MainForm();
                    Program.mainForm.PATH_FOR_SEACH_ACCOUNT_MONTH = pathForSearchAccountMonth;
                    return 0;
                }
                else
                {
                    return returnCodeFromSetup;
                }

            }
            else
            {
                Program.startForm = new DeviceForm();
                DialogResult result = Program.startForm.ShowDialog(this);
                if (result != DialogResult.OK)
                {
                    return 99;
                }
                else
                {
                    deviceID = Program.startForm.SelectedUniqueID;
                }

                WriteCDModule.Instance.Initialize(deviceID);
                int returnCodeFromSetup = SetupBeforeMainForm();
                if (returnCodeFromSetup == 0)
                {
                    Program.isContinueMainForm = true;
                    Program.mainForm = new MainForm();
                    Program.mainForm.PATH_FOR_SEACH_ACCOUNT_MONTH = pathForSearchAccountMonth;
                    return 0;
                }
                else
                {
                    return returnCodeFromSetup;
                }
            }
        }

        private int SetupBeforeMainForm()
        {
            if (Directory.Exists(pathForSearchAccountMonth))
            {
                List<string> accountMonthList = GetDirectoryList(pathForSearchAccountMonth);
                if (VerifyAccountMonthDirectory(accountMonthList))
                {
                    MapComputerWorkingStore2(accountMonthList, pathForSearchAccountMonth);
                    return 0;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                int returnTryCreateMapDrive = TryCreateMapDrive();
                if (returnTryCreateMapDrive != 0)
                {
                    return 4;
                }

                List<string> accountMonthList = GetDirectoryList(pathForSearchAccountMonth + @"\");
                if (VerifyAccountMonthDirectory(accountMonthList))
                {
                    MapComputerWorkingStore2(accountMonthList, pathForSearchAccountMonth);
                    return 0;
                }
                else
                {
                    return 5;
                }
            }
        }

        private int TryCreateMapDrive()
        {
            string emptyDriveForMap = DAOHelper.Instance.GetSettingValue("emptyDriveToMap");
            string usernameMapDrive = DAOHelper.Instance.GetSettingValue("usernameMapDrive");
            string passwordMapDrive = DAOHelper.Instance.GetSettingValue("passwordMapDrive");

            //if (!string.IsNullOrEmpty(usernameMapDrive) && !string.IsNullOrEmpty(passwordMapDrive) && !string.IsNullOrEmpty(emptyDriveForMap))
            //{
            string command2 = "NET USE " + emptyDriveForMap + " " +
            "\"" + pathForSearchAccountMonth + "\"" + " /user:" + usernameMapDrive + " " + passwordMapDrive;
            int exitCodeNetUse = ExecuteCommand(command2, 15000);
            Console.WriteLine(exitCodeNetUse);
            if (exitCodeNetUse != 0)
            {
                //return 0;
                //edit 1/25/2017
                return 4;
            }
            else
            {
                pathForSearchAccountMonth = emptyDriveForMap;
                return 0;
            }
            //}
        }

        private List<string> GetDirectoryList(string pathForSearch)
        {
            //if (Directory.Exists(pathForSearch))
            //{
            List<string> directoryList = new List<string>();
            IEnumerable<string> directoryEnum = Directory.EnumerateDirectories(pathForSearch);

            foreach (string item in directoryEnum)
            {
                string[] split = item.Split('\\');
                string forAdd = split[split.Length - 1];
                directoryList.Add(forAdd);
            }

            return directoryList;
            //}
            //else
            //{
            //    return null;
            //}
        }

        private bool VerifyAccountMonthDirectory(List<string> AccountMonthDirectory)
        {
            foreach (string item in AccountMonthDirectory)
            {
                if (item.Length != 6 || !Regex.IsMatch(item, @"^\d+$"))
                {
                    return false;
                }
            }
            return true;
        }

        private void MapComputerWorkingStore(List<string> accountMonthList, string pathForSearchAccountMonth)
        {
            foreach (string item in accountMonthList)
            {
                string year = item.Substring(0, 4);
                string month = item.Substring(4, 2);
                int yearN = Int32.Parse(year);
                int monthN = Int32.Parse(month);

                List<DataRow> tempComputerList = DAOHelper.Instance.GetComputerList(); //all computer in db
                List<DataRow> tempComputerListInMap = DAOHelper.Instance.GetMapComputerList(yearN, monthN); // all computer used in map table
                int tempCountComputer = tempComputerList.Count;
                int tempCountComputerInMap = tempComputerListInMap.Count;

                bool isModifyFCComputerTable = true;
                if (tempComputerList.Count == tempComputerListInMap.Count)
                {
                    List<string> tempComputerListS = new List<string>();
                    List<string> tempComputerListInMapS = new List<string>();

                    foreach (DataRow row in tempComputerList)
                    {
                        tempComputerListS.Add(row[1].ToString());
                    }
                    foreach (DataRow row in tempComputerListInMap)
                    {
                        tempComputerListInMapS.Add(row[0].ToString());
                    }

                    foreach (string tempItem in tempComputerListS)
                    {
                        if (!tempComputerListInMapS.Contains(tempItem))
                        {
                            break;
                        }
                        else
                        {
                            tempComputerListInMapS.Remove(tempItem);
                        }
                    }

                    if (tempComputerListInMapS.Count == 0)
                    {
                        isModifyFCComputerTable = false;
                    }
                }

                if (isModifyFCComputerTable)
                {
                    DAOHelper.Instance.ClearMapComputerWorkingStore(yearN, monthN);

                    List<DataRow> dbStoreList = DAOHelper.Instance.GetStoreList();

                    string pathForSearch = pathForSearchAccountMonth + "\\" + item;
                    List<string> folderStoreList = GetDirectoryList(pathForSearch);
                    int countFolderStore = folderStoreList.Count;

                    if (folderStoreList != null)
                    {
                        List<string> remainderStoreList = new List<string>();
                        foreach (DataRow row in dbStoreList)
                        {
                            if (!folderStoreList.Contains(row[0].ToString()))
                            {
                                remainderStoreList.Add(row[0].ToString());
                            }
                            else
                            {
                                continue;
                            }
                        }
                        int countRemainderStore = remainderStoreList.Count;

                        List<DataRow> computerList = DAOHelper.Instance.GetComputerList();
                        int countComputer = computerList.Count;

                        if (remainderStoreList.Count > 0)
                        {
                            foreach (DataRow row in computerList)
                            {
                                int range = countRemainderStore / countComputer;
                                int remainder = countRemainderStore % countComputer;
                                if (remainder > 0 && (int)row[0] <= remainder)
                                {
                                    range = range + 1;
                                }

                                List<string> storeToMap = new List<string>();
                                for (int i = 1; i <= range; i++)
                                {
                                    string store = remainderStoreList[0];
                                    storeToMap.Add(store);
                                    remainderStoreList.RemoveAt(0);
                                }
                                if (storeToMap.Count > 0)
                                {
                                    DAOHelper.Instance.Execute_SP_MapComputerWorkingStore(storeToMap, row[1].ToString(), yearN, monthN);
                                }
                            }
                        }

                        if (folderStoreList.Count > 0)
                        {
                            foreach (DataRow row in computerList)
                            {
                                int range = countFolderStore / countComputer;
                                int remainder = countFolderStore % countComputer;
                                if (remainder > 0 && (int)row[0] <= remainder)
                                {
                                    range = range + 1;
                                }

                                List<string> storeToMap = new List<string>();
                                for (int i = 1; i <= range; i++)
                                {
                                    string store = folderStoreList[0];
                                    storeToMap.Add(store);
                                    folderStoreList.RemoveAt(0);
                                }

                                if (storeToMap.Count > 0)
                                {
                                    DAOHelper.Instance.Execute_SP_MapComputerWorkingStore(storeToMap, row[1].ToString(), yearN, monthN);
                                }
                            }
                        }
                    }
                }


            }
            //////
        }

        private void MapComputerWorkingStore2(List<string> accountMonthList, string pathForSearchAccountMonth)
        {
            foreach (string item in accountMonthList)
            {
                string year = item.Substring(0, 4);
                string month = item.Substring(4, 2);
                int yearN = Int32.Parse(year);
                int monthN = Int32.Parse(month);

                List<DataRow> tempComputerList = DAOHelper.Instance.GetComputerList(); //all computer in db
                List<DataRow> tempComputerListInMap = DAOHelper.Instance.GetMapComputerList(yearN, monthN); // all computer used in map table
                int tempCountComputer = tempComputerList.Count;
                int tempCountComputerInMap = tempComputerListInMap.Count;

                bool isModifyFCComputerTable = true;
                if (tempComputerList.Count == tempComputerListInMap.Count)
                {
                    List<string> tempComputerListS = new List<string>();
                    List<string> tempComputerListInMapS = new List<string>();

                    foreach (DataRow row in tempComputerList)
                    {
                        tempComputerListS.Add(row[1].ToString());
                    }
                    foreach (DataRow row in tempComputerListInMap)
                    {
                        tempComputerListInMapS.Add(row[0].ToString());
                    }

                    foreach (string tempItem in tempComputerListS)
                    {
                        if (!tempComputerListInMapS.Contains(tempItem))
                        {
                            break;
                        }
                        else
                        {
                            tempComputerListInMapS.Remove(tempItem);
                        }
                    }

                    if (tempComputerListInMapS.Count == 0)
                    {
                        isModifyFCComputerTable = false;
                    }
                }

                if (isModifyFCComputerTable)
                {
                    DAOHelper.Instance.ClearMapComputerWorkingStore(yearN, monthN);

                    List<DataRow> dbStoreList = DAOHelper.Instance.GetStoreList();

                    List<string> copyDBStoreList = new List<string>();
                    foreach (DataRow row in dbStoreList)
                    {
                        copyDBStoreList.Add(row[0].ToString());
                    }

                    string pathForSearch = pathForSearchAccountMonth + "\\" + item;
                    List<string> folderStoreList = GetDirectoryList(pathForSearch);
                    int countFolderStore = folderStoreList.Count;

                    if (folderStoreList != null)
                    {
                        List<string> orderedStoreList = new List<string>();
                        foreach (DataRow row in dbStoreList)
                        {
                            if (folderStoreList.Contains(row[0].ToString()))
                            {
                                orderedStoreList.Add(row[0].ToString());
                                copyDBStoreList.Remove(row[0].ToString());
                            }
                            else
                            {
                                continue;
                            }
                        }

                        foreach (string storeCode in copyDBStoreList)
                        {
                            orderedStoreList.Add(storeCode);
                        }

                        int countOrderedStore = orderedStoreList.Count;

                        List<DataRow> computerList = DAOHelper.Instance.GetComputerList();
                        int countComputer = computerList.Count;

                        List<string>[] arOfListStore = new List<string>[countComputer];
                        for (int i = 0; i < arOfListStore.Length; i++)
                        {
                            arOfListStore[i] = new List<string>();
                        }

                        if (countOrderedStore > 0)
                        {
                            int currentIndexAr = 0;
                            foreach (string row in orderedStoreList)
                            {
                                arOfListStore[currentIndexAr].Add(row);
                                if (currentIndexAr == arOfListStore.Length - 1)
                                {
                                    currentIndexAr = 0;
                                }
                                else
                                {
                                    currentIndexAr++;
                                }
                            }

                            currentIndexAr = 0;
                            foreach (DataRow row in computerList)
                            {
                                DAOHelper.Instance.Execute_SP_MapComputerWorkingStore(arOfListStore[currentIndexAr], row[1].ToString(), yearN, monthN);
                                currentIndexAr++;
                            }
                        }
                    }
                }


            }
            //////
        }

        private void LandingForm_Shown(object sender, EventArgs e)
        {
            timerInitialize.Tick += timerInitialize_Tick;
            timerInitialize.Start();
        }

        void timerInitialize_Tick(object sender, EventArgs e)
        {
            timerInitialize.Stop();
            timerInitialize.Dispose();
            int returnCode = Initialize();
            if (returnCode != 0)
            {
                ErrorMessage er = new ErrorMessage();
                er.Show(this, returnCode);
                Environment.Exit(returnCode);
            }
            this.Dispose();
        }

        private int ExecuteCommand(string command, int timeout)
        {
            try
            {
                var processInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = "C:\\",
                };

                var process = Process.Start(processInfo);
                process.WaitForExit(timeout);
                var exitCode = process.ExitCode;
                process.Close();
                return exitCode;
            }
            catch (Exception ex)
            {
                return 1;
            }

        }
    }
}
