using IMAPI2.Interop;
using IMAPI2.MediaItem;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace FCWriteCD.Module
{
    class WriteCDModule
    {
        //////////
        private static WriteCDModule instance;
        private WriteCDModule() { }
        public static WriteCDModule Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WriteCDModule();
                }
                return instance;
            }
        }
        /////////

        private const string CLIENT_NAME = "FCWriteCD";

        private IMAPI_BURN_VERIFICATION_LEVEL _verificationLevel =
    IMAPI_BURN_VERIFICATION_LEVEL.IMAPI_BURN_VERIFICATION_NONE;

        private string UniqueID = null;
        public string UNIQUE_ID
        {
            get { return UniqueID; }
        }

        private BurnData burnData;

        private bool isCloseMedia;

        private bool isEjectMedia;

        private bool isRequestCancel;
        public bool IS_REQUEST_CANCEL
        {
            get { return isRequestCancel; }
            set { isRequestCancel = value; }
        }

        public void Initialize(string selectedUniqueID)
        {
            UniqueID = selectedUniqueID;
            burnData = new BurnData();
            isCloseMedia = false;
            isEjectMedia = true;
            isRequestCancel = false;
        }

        public string DetectMedia()
        {
            MsftDiscFormat2Data discFormatData = null;
            MsftDiscRecorder2 discRecorder = null;

            try
            {
                discRecorder = new MsftDiscRecorder2();
                discRecorder.InitializeDiscRecorder(UniqueID);

                discFormatData = new MsftDiscFormat2Data();
                if (!discFormatData.IsCurrentMediaSupported(discRecorder))
                {
                    return "ไม่มีแผ่นซีดีหรือแผ่นซีดีไม่ถูกต้อง";
                }
                else
                {
                    discFormatData.Recorder = discRecorder;
                    IMAPI_MEDIA_PHYSICAL_TYPE mediaType = discFormatData.CurrentPhysicalMediaType;
                    string mediaTypeValue = GetMediaTypeString(mediaType);
                    if (!(mediaTypeValue.Contains("CD-R")
                            || mediaTypeValue.Contains("CD-RW")))
                    {
                        return "กรุณาใส่แผ่นซีดีประเภท CD-R หรือ CD-RW เท่านั้น";
                    }
                }
            }
            catch (COMException exception)
            {
                return "Detect media error";
            }
            finally
            {
                if (discFormatData != null)
                {
                    Marshal.ReleaseComObject(discFormatData);
                }
            }

            return string.Empty;
        }

        public Tuple<string, Int64> DetectSpace()
        {
            MsftFileSystemImage fileSystemImage = null;
            MsftDiscFormat2Data discFormatData = null;
            MsftDiscRecorder2 discRecorder = null;

            try
            {
                discRecorder = new MsftDiscRecorder2();
                discRecorder.InitializeDiscRecorder(UniqueID);

                discFormatData = new MsftDiscFormat2Data();
                discFormatData.Recorder = discRecorder;

                fileSystemImage = new MsftFileSystemImage();
                IMAPI_MEDIA_PHYSICAL_TYPE mediaType = discFormatData.CurrentPhysicalMediaType;
                fileSystemImage.ChooseImageDefaultsForMediaType(mediaType);

                //
                // See if there are other recorded sessions on the disc
                //
                if (!discFormatData.MediaHeuristicallyBlank)
                {
                    fileSystemImage.MultisessionInterfaces = discFormatData.MultisessionInterfaces;
                    fileSystemImage.ImportFileSystem();
                }

                Int64 freeMediaBlocks = fileSystemImage.FreeMediaBlocks;
                Int64 _totalDiscSize = 2048 * freeMediaBlocks;
                return new Tuple<string, Int64>(string.Empty, _totalDiscSize);
            }
            catch (COMException exception)
            {
                Tuple<string, Int64> returnValue = new Tuple<string, Int64>("Detect space error", 0);
                return returnValue;
                //return "Detect space error";
                //MessageBox.Show(this, exception.Message, "Detect Media Error",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public void EjectMedia()
        {
            MsftDiscRecorder2 discRecorder = null;
            discRecorder = new MsftDiscRecorder2();
            discRecorder.InitializeDiscRecorder(UniqueID);
            discRecorder.EjectMedia();
        }

        private string CreateMediaFileSystem(object[] multisessionInterfaces, out IStream dataStream,
            string volumeName, string pathForMediaItem)
        {
            MsftFileSystemImage fileSystemImage = null;
            MsftDiscRecorder2 discRecorder = null;

            try
            {
                discRecorder = new MsftDiscRecorder2();
                discRecorder.InitializeDiscRecorder(UniqueID);

                fileSystemImage = new MsftFileSystemImage();
                fileSystemImage.ChooseImageDefaults(discRecorder);
                fileSystemImage.FileSystemsToCreate =
                    FsiFileSystems.FsiFileSystemJoliet | FsiFileSystems.FsiFileSystemISO9660;
                fileSystemImage.VolumeName = volumeName;

                //
                // If multisessions, then import previous sessions
                //
                if (multisessionInterfaces != null)
                {
                    fileSystemImage.MultisessionInterfaces = multisessionInterfaces;
                    fileSystemImage.ImportFileSystem();
                }

                //
                // Get the image root
                //
                IFsiDirectoryItem rootItem = fileSystemImage.Root;
                //
                // Add Files and Directories to File System Image
                //
                //
                // Check if we've cancelled
                //
                //
                // Add to File System
                //

                var directoryItem = new DirectoryItem(pathForMediaItem);
                IMediaItem mediaItem = directoryItem;
                if (!mediaItem.AddToFileSystem(rootItem))
                {
                    dataStream = null;
                    return "มีโฟลเดอร์หรือไฟล์ที่ต้องการเขียนอยู่ในแผ่นซีดีแล้ว";
                }


                //
                // did we cancel?
                //
                if (isRequestCancel)
                {
                    dataStream = null;
                    return "เกิดคำสั่งยกเลิกการเขียนแผ่นซีดี";
                }

                dataStream = fileSystemImage.CreateResultImage().ImageStream;
            }
            catch (COMException exception)
            {
                //MessageBox.Show(this, exception.Message, "Create File System Error",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataStream = null;
                return "Create File System Error";
            }
            finally
            {
                if (fileSystemImage != null)
                {
                    Marshal.ReleaseComObject(fileSystemImage);
                }
            }

            return string.Empty;
        }

        public string BurnCD(string volumeName, string pathForMediaItem)
        {
            string returnString = DoBurn(volumeName, pathForMediaItem);
            if (string.Empty.Equals(returnString))
            {
                EndBurn();
                return returnString;
            }
            else
            {
                return returnString;
            }
        }

        private string DoBurn(string volumeName, string pathForMediaItem)
        {
            MsftDiscRecorder2 discRecorder = null;
            MsftDiscFormat2Data discFormatData = null;

            try
            {

                discRecorder = new MsftDiscRecorder2();
                discRecorder.InitializeDiscRecorder(UniqueID);


                discFormatData = new MsftDiscFormat2Data
                {
                    Recorder = discRecorder,
                    ClientName = CLIENT_NAME,
                    ForceMediaToBeClosed = isCloseMedia
                };


                var burnVerification = (IBurnVerification)discFormatData;
                burnVerification.BurnVerificationLevel = _verificationLevel;


                object[] multisessionInterfaces = null;
                if (!discFormatData.MediaHeuristicallyBlank)
                {
                    multisessionInterfaces = discFormatData.MultisessionInterfaces;
                }


                IStream fileSystem;
                string errorMessage = CreateMediaFileSystem(multisessionInterfaces,
                    out fileSystem, volumeName, pathForMediaItem);
                if (!string.Empty.Equals(errorMessage))
                {
                    //e.Result = -1;
                    return errorMessage;
                }


                discFormatData.Update += discFormatData_Update;


                try
                {
                    discFormatData.Write(fileSystem);
                    //e.Result = 0;
                }
                catch (COMException ex)
                {
                    return "Error Code = " + ex.ErrorCode.ToString();
                    //e.Result = ex.ErrorCode;
                    //MessageBox.Show(ex.Message, "IDiscFormat2Data.Write failed",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    if (fileSystem != null)
                    {
                        Marshal.FinalReleaseComObject(fileSystem);
                    }
                }


                discFormatData.Update -= discFormatData_Update;

                if (isEjectMedia)
                {
                    discRecorder.EjectMedia();
                }
            }
            catch (COMException exception)
            {
                return "Error Code : " + exception.ErrorCode;
                //MessageBox.Show(exception.Message);
                //e.Result = exception.ErrorCode;
            }
            finally
            {
                if (discRecorder != null)
                {
                    Marshal.ReleaseComObject(discRecorder);
                }

                if (discFormatData != null)
                {
                    Marshal.ReleaseComObject(discFormatData);
                }
            }

            return string.Empty;
        }

        private void discFormatData_Update([In, MarshalAs(UnmanagedType.IDispatch)] object sender,
            [In, MarshalAs(UnmanagedType.IDispatch)] object progress)
        {
            //
            // Check if we've cancelled
            //
            if (isRequestCancel)
            {
                var format2Data = (IDiscFormat2Data)sender;
                format2Data.CancelWrite();
                return;
            }

            var eventArgs = (IDiscFormat2DataEventArgs)progress;

            burnData.task = BURN_MEDIA_TASK.BURN_MEDIA_TASK_WRITING;

            // IDiscFormat2DataEventArgs Interface
            burnData.elapsedTime = eventArgs.ElapsedTime;
            burnData.remainingTime = eventArgs.RemainingTime;
            burnData.totalTime = eventArgs.TotalTime;

            // IWriteEngine2EventArgs Interface
            burnData.currentAction = eventArgs.CurrentAction;
            burnData.startLba = eventArgs.StartLba;
            burnData.sectorCount = eventArgs.SectorCount;
            burnData.lastReadLba = eventArgs.LastReadLba;
            burnData.lastWrittenLba = eventArgs.LastWrittenLba;
            burnData.totalSystemBuffer = eventArgs.TotalSystemBuffer;
            burnData.usedSystemBuffer = eventArgs.UsedSystemBuffer;
            burnData.freeSystemBuffer = eventArgs.FreeSystemBuffer;

            //
            // Report back to the UI
            //
            //backgroundBurnWorker.ReportProgress(0, _burnData);
        }

        private void EndBurn()
        {

        }

        private string GetMediaTypeString(IMAPI_MEDIA_PHYSICAL_TYPE mediaType)
        {
            switch (mediaType)
            {
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_UNKNOWN:
                default:
                    return "Unknown Media Type";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_CDROM:
                    return "CD-ROM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_CDR:
                    return "CD-R";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_CDRW:
                    return "CD-RW";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDROM:
                    return "DVD ROM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDRAM:
                    return "DVD-RAM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSR:
                    return "DVD+R";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSRW:
                    return "DVD+RW";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSR_DUALLAYER:
                    return "DVD+R Dual Layer";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDDASHR:
                    return "DVD-R";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDDASHRW:
                    return "DVD-RW";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDDASHR_DUALLAYER:
                    return "DVD-R Dual Layer";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DISK:
                    return "random-access writes";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSRW_DUALLAYER:
                    return "DVD+RW DL";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_HDDVDROM:
                    return "HD DVD-ROM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_HDDVDR:
                    return "HD DVD-R";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_HDDVDRAM:
                    return "HD DVD-RAM";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_BDROM:
                    return "Blu-ray DVD (BD-ROM)";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_BDR:
                    return "Blu-ray media";

                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_BDRE:
                    return "Blu-ray Rewritable media";
            }
        }
    }
}
