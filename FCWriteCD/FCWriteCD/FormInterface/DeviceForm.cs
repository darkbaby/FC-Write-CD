using FCWriteCD.Module;
using IMAPI2.Interop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FCWriteCD.FormInterface
{
    public partial class DeviceForm : Form
    {
        private bool isContinueNextForm;
        public bool IS_CONTINUE_NEXT_FORM
        {
            get { return isContinueNextForm; }
        }

        private string selectedUniqueID;
        public string SelectedUniqueID
        {
            get { return selectedUniqueID; }
        }

        public DeviceForm()
        {
            InitializeComponent();
            isContinueNextForm = false;
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            MsftDiscMaster2 discMaster = null;
            try
            {
                discMaster = new MsftDiscMaster2();

                if (!discMaster.IsSupportedEnvironment)
                    return;
                foreach (string uniqueRecorderId in discMaster)
                {
                    var discRecorder2 = new MsftDiscRecorder2();
                    discRecorder2.InitializeDiscRecorder(uniqueRecorderId);

                    ComboBoxItem cbItem = new ComboBoxItem();
                    string name = (string)discRecorder2.VolumePathNames.GetValue(0) + "" + discRecorder2.ProductId;
                    cbItem.Key = name.Trim();
                    cbItem.Value = uniqueRecorderId;
                    cbItem.Disc = discRecorder2;
                    devicesComboBox.Items.Add(cbItem);

                }
                if (devicesComboBox.Items.Count > 0)
                {
                    devicesComboBox.SelectedIndex = 0;
                    selectedUniqueID = ((ComboBoxItem)devicesComboBox.Items[devicesComboBox.SelectedIndex]).Value;
                }
            }
            catch (COMException ex)
            {
                MessageBox.Show(ex.Message,
                    string.Format("Error:{0} - Please install IMAPI2", ex.ErrorCode),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            finally
            {
                if (discMaster != null)
                {
                    Marshal.ReleaseComObject(discMaster);
                }
            }
        }

        private void devicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (devicesComboBox.SelectedIndex == -1)
            {
                return;
            }

            var discRecorder =
                (IDiscRecorder2)((ComboBoxItem)devicesComboBox.Items[devicesComboBox.SelectedIndex]).Disc;

            labelSupportMedia.Text = string.Empty;

            IDiscFormat2Data discFormatData = null;
            try
            {
                StringBuilder supportedMediaTypes = new StringBuilder();
                foreach (IMAPI_PROFILE_TYPE profileType in discRecorder.SupportedProfiles)
                {
                    string profileName = GetProfileTypeString(profileType);

                    if (string.IsNullOrEmpty(profileName))
                        continue;

                    if (supportedMediaTypes.Length > 0)
                        supportedMediaTypes.Append(", ");
                    supportedMediaTypes.Append(profileName);
                }

                labelSupportMedia.Text = supportedMediaTypes.ToString();

                selectedUniqueID = ((ComboBoxItem)devicesComboBox.Items[devicesComboBox.SelectedIndex]).Value;
            }
            catch (COMException)
            {
                labelSupportMedia.Text = "Error getting supported types";
            }
            finally
            {
                if (discFormatData != null)
                {
                    Marshal.ReleaseComObject(discFormatData);
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string supportMedia = labelSupportMedia.Text;
            if (supportMedia.Contains("CD-R")
                    || supportMedia.Contains("CD-RW"))
            {
                DAOHelper.Instance.UpdateDeviceID(Environment.MachineName, selectedUniqueID);
                //WriteCDModule.Instance.Initialize(selectedUniqueID);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Device ของคุณไม่เหมาะสมกับโปรแกรม");
            }
        }

        private string GetProfileTypeString(IMAPI_PROFILE_TYPE profileType)
        {
            switch (profileType)
            {
                default:
                    return string.Empty;

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_CD_RECORDABLE:
                    return "CD-R";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_CD_REWRITABLE:
                    return "CD-RW";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVDROM:
                    return "DVD ROM";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_RECORDABLE:
                    return "DVD-R";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_RAM:
                    return "DVD-RAM";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_PLUS_R:
                    return "DVD+R";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_PLUS_RW:
                    return "DVD+RW";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_PLUS_R_DUAL:
                    return "DVD+R Dual Layer";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_REWRITABLE:
                    return "DVD-RW";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_RW_SEQUENTIAL:
                    return "DVD-RW Sequential";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_R_DUAL_SEQUENTIAL:
                    return "DVD-R DL Sequential";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_DASH_R_DUAL_LAYER_JUMP:
                    return "DVD-R Dual Layer";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_DVD_PLUS_RW_DUAL:
                    return "DVD+RW DL";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_HD_DVD_ROM:
                    return "HD DVD-ROM";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_HD_DVD_RECORDABLE:
                    return "HD DVD-R";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_HD_DVD_RAM:
                    return "HD DVD-RAM";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_BD_ROM:
                    return "Blu-ray DVD (BD-ROM)";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_BD_R_SEQUENTIAL:
                    return "Blu-ray media Sequential";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_BD_R_RANDOM_RECORDING:
                    return "Blu-ray media";

                case IMAPI_PROFILE_TYPE.IMAPI_PROFILE_TYPE_BD_REWRITABLE:
                    return "Blu-ray Rewritable media";
            }
        }

    }
}
