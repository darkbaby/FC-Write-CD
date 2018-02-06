using FCWriteCD.CustomColumn;
using FCWriteCD.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCWriteCD.FormInterface
{
    public partial class MainForm : Form
    {
        private bool isStart = false;

        //private bool isDiscInserted;
        //public bool IS_DISC_INSERTED
        //{
        //    get
        //    {
        //        return isDiscInserted;
        //    }
        //    set
        //    {
        //        isDiscInserted = value;
        //    }
        //}

        private string pathForSearchAccountMonth;
        public string PATH_FOR_SEACH_ACCOUNT_MONTH
        {
            get { return pathForSearchAccountMonth; }
            set { pathForSearchAccountMonth = value; }
        }

        private DiscDeviceWatcher discDeviceWatcher;

        private DialogForm dialogForm;

        private ImageList imageList;

        private int selectedMonth;

        private int selectedYear;

        private string selectedMonthS;

        private string selectedYearS;

        private string selectedComputerNameFilter;

        private int lineCode = 1;

        private readonly string computerName = Environment.MachineName;
        //private readonly string computerName = "NOPPAKAO-NB";
        //private readonly string computerName = "DREAM-NB";

        private FileNameUtil fileNameUtil = new FileNameUtil();

        private string CDName;

        public MainForm()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            this.MaximizeBox = false;
            dialogForm = new DialogForm(this);
            InitializeUI();
            PrepareForm();
        }

        private void InitializeUI()
        {
            for (int i = 1; i <= 12; i++)
            {
                switch (i)
                {
                    case 1:
                        comboBoxMonth.Items.Add("01 : มกราคม");
                        break;
                    case 2:
                        comboBoxMonth.Items.Add("02 : กุมภาพันธ์");
                        break;
                    case 3:
                        comboBoxMonth.Items.Add("03 : มีนาคม");
                        break;
                    case 4:
                        comboBoxMonth.Items.Add("04 : เมษายน");
                        break;
                    case 5:
                        comboBoxMonth.Items.Add("05 : พฤษภาคม");
                        break;
                    case 6:
                        comboBoxMonth.Items.Add("06 : มิถุนายน");
                        break;
                    case 7:
                        comboBoxMonth.Items.Add("07 : กรกฎาคม");
                        break;
                    case 8:
                        comboBoxMonth.Items.Add("08 : สิงหาคม");
                        break;
                    case 9:
                        comboBoxMonth.Items.Add("09 : กันยายน");
                        break;
                    case 10:
                        comboBoxMonth.Items.Add("10 : ตุลาคม");
                        break;
                    case 11:
                        comboBoxMonth.Items.Add("11 : พฤศจิกายน");
                        break;
                    case 12:
                        comboBoxMonth.Items.Add("12 : ธันวาคม");
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i <= 1; i++)
            {
                comboBoxYear.Items.Add(DateTime.Now.AddYears(-1).Year + i);
            }

            if (DateTime.Now.Month - 1 == 0)
            {
                comboBoxMonth.SelectedIndex = 11;
                comboBoxYear.SelectedIndex = 0;
            }
            else
            {
                comboBoxMonth.SelectedIndex = DateTime.Now.Month - 2;
                comboBoxYear.SelectedIndex = 1;
            }

            comboBoxFilter.Items.Add("All");
            foreach (DataRow row in DAOHelper.Instance.GetComputerList())
            {
                comboBoxFilter.Items.Add(row[1].ToString());
            }

            int InitialIndexComboBoxFilter = 0;
            foreach (var item in comboBoxFilter.Items)
            {
                if (computerName.Equals(item.ToString()))
                {
                    selectedComputerNameFilter = computerName;
                    break;
                }
                InitialIndexComboBoxFilter++;
            }

            comboBoxFilter.SelectedIndex = InitialIndexComboBoxFilter;
        }

        void PrepareForm()
        {
            imageList = new ImageList();
            imageList.Images.Add("S", Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"\Icon\Success.png"));
            imageList.Images.Add("E", Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"\Icon\Error.png"));
            imageList.Images.Add("P", Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"\Icon\OnProgress.png"));

            selectedMonth = -1;
            selectedYear = -1;
            selectedMonthS = comboBoxMonth.Items[comboBoxMonth.SelectedIndex].ToString().Substring(0, 2);
            selectedYearS = comboBoxYear.Items[comboBoxYear.SelectedIndex].ToString();

            discDeviceWatcher = new DiscDeviceWatcher(OnInsertDisc, OnEjectDisc);
            //isDiscInserted = discDeviceWatcher.DetectCDOnDevice();

            IniModule iniModule = new IniModule(AppDomain.CurrentDomain.BaseDirectory + @"\setting.ini");
            CDName = DateTime.Now.ToString("yyyy-MM-dd");
            CDName = iniModule.IniReadValueSpecial("Output", "CDName");

            DAOHelper.Instance.InsertLog(computerName, Log_Type.Process, "== START PROGRAM ==", lineCode++, 0);
        }

        #region Form Component Event

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            selectedMonthS = comboBoxMonth.Items[comboBoxMonth.SelectedIndex].ToString().Substring(0, 2);
            selectedYearS = comboBoxYear.Items[comboBoxYear.SelectedIndex].ToString();

            if (selectedMonth == Int32.Parse(selectedMonthS) && selectedYear == Int32.Parse(selectedYearS))
            {
                return;
            }
            else
            {
                selectedMonth = Int32.Parse(selectedMonthS);
                selectedYear = Int32.Parse(selectedYearS);
            }

            dgvStore.Rows.Clear();
            if (dgvWrote.DataSource != null)
            {
                (dgvWrote.DataSource as DataTable).Rows.Clear();
            }

            string pathForSearch = pathForSearchAccountMonth + "\\" + selectedYearS + selectedMonthS;
            List<string> folderStoreList = GetDirectoryList(pathForSearch);

            if (folderStoreList != null)
            {
                ////
                List<DataRow> FinishedStoreList = DAOHelper.Instance.GetWroteStoreList(selectedYear, selectedMonth, "All").AsEnumerable().ToList();
                List<string> FinishedStoreListString = ConvertToListString(FinishedStoreList, 0);
                List<DataRow> FinishedStoreList2 = DAOHelper.Instance.Execute_SP_CustomSelectStoreName(FinishedStoreListString);

                var temp1 = (from rec in FinishedStoreList.AsEnumerable().ToList()
                             join rec2 in FinishedStoreList2 on rec[0].ToString() equals rec2[0].ToString()
                             select new { rec, rec2 });

                List<DataRow> workingStoreList = DAOHelper.Instance.GetMapComputerWorkingStore(selectedYear, selectedMonth,
                    computerName);
                List<string> workingStoreListString = ConvertToListString(workingStoreList, 3);
                workingStoreList = DAOHelper.Instance.Execute_SP_CustomSelectStoreName(workingStoreListString);

                Console.WriteLine(workingStoreList.Count());
                foreach (DataRow item in workingStoreList)
                {
                    if (!FinishedStoreListString.Contains(item[0].ToString()))
                    {
                        if (folderStoreList.Contains(item[0].ToString().Substring(0, 4)))
                        {
                            DataGridViewRow row = (DataGridViewRow)dgvStore.RowTemplate.Clone();
                            row.CreateCells(dgvStore, false, item[0].ToString() + " - " + item[1].ToString(),
                                "", "รอการดำเนินการ", "0");
                            dgvStore.Rows.Add(row);
                            Image tempImage = (Image)imageList.Images[0];
                            tempImage.Tag = "Center";
                            ((TextAndImageCell)row.Cells[2]).Image = tempImage;
                            SetRowReadOnlyProperty(row, false, false, true, true, true);
                        }
                        else
                        {
                            DataGridViewRow row = (DataGridViewRow)dgvStore.RowTemplate.Clone();
                            row.CreateCells(dgvStore, false, item[0].ToString() + " - " + item[1].ToString(),
                                "", "รอการดำเนินการ", "0");
                            dgvStore.Rows.Add(row);
                            SetRowReadOnlyProperty(row, true, true, true, true, true);
                        }
                    }
                }

                DataTable dgvWroteDataSource = new DataTable();
                dgvWroteDataSource.Columns.Add("StoreNameWrote", typeof(string));
                dgvWroteDataSource.Columns.Add("ComputerNameWrote", typeof(string));
                dgvWroteDataSource.Columns.Add("FinishTime", typeof(DateTime));
                foreach (var item in temp1)
                {
                    DataRow newRow = dgvWroteDataSource.NewRow();
                    newRow[0] = item.rec[0].ToString() + " - " + item.rec2[1].ToString();
                    newRow[1] = item.rec[1].ToString();
                    DateTime tempDateTime = DateTime.Parse(item.rec[2].ToString());
                    newRow[2] = tempDateTime;
                    //newRow[2] = tempDateTime.ToString("dd/MM/yyyy HH:mm:ss");
                    dgvWroteDataSource.Rows.Add(newRow);
                }
                dgvWrote.DataSource = dgvWroteDataSource;

                if (selectedComputerNameFilter.Equals("All"))
                {
                    (dgvWrote.DataSource as DataTable).DefaultView.RowFilter = null;
                }
                else
                {
                    (dgvWrote.DataSource as DataTable).DefaultView.RowFilter = string.Format("ComputerNameWrote = '{0}'", selectedComputerNameFilter);
                }

                (dgvWrote.DataSource as DataTable).DefaultView.Sort = "FinishTime DESC";
                ////

                DataGridViewRow tempRow = (DataGridViewRow)dgvStore.RowTemplate.Clone();
                tempRow.CreateCells(dgvStore, null, "กรุณากรอกรหัสร้านสาขาและกด Enter", "", "", "1");
                dgvStore.Rows.Add(tempRow);
                tempRow.Cells[1].Style.ForeColor = Color.Gray;
                //tempRow.Cells[1].Style = new DataGridViewCellStyle { ForeColor = Color.Green };
                //tempRow.DefaultCellStyle.BackColor = Color.LightGray;
                SetRowReadOnlyProperty(tempRow, false, true, false, true, true);

                comboBoxFilter.Enabled = true;
                buttonRefresh.Enabled = true;
                headerCheckbox.Checked = false;
                headerCheckbox.Enabled = true;

                dgvStore.Focus();
            }
            else
            {
                comboBoxFilter.Enabled = false;
                buttonRefresh.Enabled = false;
                headerCheckbox.Checked = false;
                headerCheckbox.Enabled = false;
                DAOHelper.Instance.InsertLog(computerName, Log_Type.Directory, "== DIDNT FIND THE GIVEN FOLDER PATH (" + pathForSearch + ") ==", lineCode++, 0);
                ShowMessageBox(false, "ไม่พบโฟลเดอร์ปลายทาง", "ข้อผิดพลาด", MessageBoxButtons.OK);
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (dgvStore.Rows.Count == 0)
            {
                return;
            }

            string month = selectedMonthS;
            string year = selectedYearS;

            List<DataGridViewRow> selectedStore = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvStore.Rows)
            {
                if (row.Cells[0].Value == null || row.ReadOnly)
                {
                    continue;
                }

                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    selectedStore.Add(row);
                }
            }
            if (selectedStore.Count > 0)
            {
                isStart = true;
                WriteCDModule.Instance.IS_REQUEST_CANCEL = false;
                DAOHelper.Instance.InsertLog(computerName, Log_Type.Disc, "== START BURN CD ==", lineCode++, 0);
                DisableUI();
                BurnAsync(month, year, selectedStore);
            }
            else
            {
                ShowMessageBox(true, "กรุณาเลือกร้านสาขาที่ต้องการ", "แจ้งเตือน", MessageBoxButtons.OK);
            }

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            buttonStop.Enabled = false;
            isStart = false;
            WriteCDModule.Instance.IS_REQUEST_CANCEL = true;
            DAOHelper.Instance.InsertLog(computerName, Log_Type.Disc, "== CANCEL BURN REQUESTED ==", lineCode++, 0);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            if (dgvWrote.DataSource != null)
            {
                (dgvWrote.DataSource as DataTable).Rows.Clear();
            }

            List<DataRow> FinishedStoreList = DAOHelper.Instance.GetWroteStoreList(selectedYear, selectedMonth, "All").AsEnumerable().ToList();
            List<string> FinishedStoreListString = ConvertToListString(FinishedStoreList, 0);
            List<DataRow> FinishedStoreList2 = DAOHelper.Instance.Execute_SP_CustomSelectStoreName(FinishedStoreListString);

            var temp1 = (from rec in FinishedStoreList.AsEnumerable().ToList()
                         join rec2 in FinishedStoreList2 on rec[0].ToString() equals rec2[0].ToString()
                         select new { rec, rec2 });

            DataTable dgvWroteDataSource = new DataTable();
            dgvWroteDataSource.Columns.Add("StoreNameWrote", typeof(string));
            dgvWroteDataSource.Columns.Add("ComputerNameWrote", typeof(string));
            dgvWroteDataSource.Columns.Add("FinishTime", typeof(string));
            foreach (var item in temp1)
            {
                DataRow newRow = dgvWroteDataSource.NewRow();
                newRow[0] = item.rec[0].ToString() + " - " + item.rec2[1].ToString();
                newRow[1] = item.rec[1].ToString();
                DateTime tempDateTime = DateTime.Parse(item.rec[2].ToString());
                newRow[2] = tempDateTime.ToString("dd/MM/yyyy HH:mm:ss");
                dgvWroteDataSource.Rows.Add(newRow);
            }

            dgvWrote.DataSource = dgvWroteDataSource;

            if (selectedComputerNameFilter.Equals("All"))
            {
                (dgvWrote.DataSource as DataTable).DefaultView.RowFilter = null;
            }
            else
            {
                (dgvWrote.DataSource as DataTable).DefaultView.RowFilter = string.Format("ComputerNameWrote = '{0}'", selectedComputerNameFilter);
            }

            dgvWrote.Focus();
        }

        private void headerCheckbox_Click(object sender, EventArgs e)
        {
            bool isChecked = ((CheckBox)sender).Checked;

            foreach (DataGridViewRow row in dgvStore.Rows)
            {
                if (row.Cells[0].Value == null)
                {
                    continue;
                }

                if (!row.ReadOnly)
                {
                    row.Cells[0].Value = isChecked;
                }
            }
        }

        private void comboBoxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMonth.Items.Count < 12)
            {
                return;
            }
            else
            {
                buttonSearch.Focus();
            }
        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxYear.Items.Count < 2)
            {
                return;
            }
            else
            {
                buttonSearch.Focus();
            }
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedComputerNameFilter = comboBoxFilter.Items[comboBoxFilter.SelectedIndex].ToString();
            if (dgvWrote.DataSource == null)
            {
                return;
            }
            if (selectedComputerNameFilter.Equals("All"))
            {
                (dgvWrote.DataSource as DataTable).DefaultView.RowFilter = null;
            }
            else
            {
                (dgvWrote.DataSource as DataTable).DefaultView.RowFilter = string.Format("ComputerNameWrote = '{0}'", selectedComputerNameFilter);
            }

            //(dgvWrote.DataSource as DataTable).DefaultView.Sort = "FinishTime DESC";

            dgvWrote.Focus();
        }

        private void dgvStore_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = string.Empty;
                dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
            }
        }

        private void dgvStore_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvStore.CurrentCell.ColumnIndex == 1)
            {
                e.Control.KeyPress -= new KeyPressEventHandler(StoreName_Validate_KeyPress);
                TextBox tb = e.Control as TextBox;
                tb.MaxLength = 4;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(StoreName_Validate_KeyPress);
                }
            }
        }

        private void StoreName_Validate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvStore_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                {
                    dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                    return;
                }

                string temp = dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                string fullStoreName = DAOHelper.Instance.GetStoreName(temp);

                if (string.IsNullOrEmpty(temp.Trim()))
                {
                    dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                    dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "กรุณากรอกรหัสร้านสาขาและกด Enter";
                    return;
                }
                else if (fullStoreName == null && temp.Trim().Length == 4)
                {
                    dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                    dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "กรุณากรอกรหัสร้านสาขาและกด Enter";
                    ShowMessageBox(false, "ไม่พบร้านสาขาที่กรอก", "ข้อผิดพลาด", MessageBoxButtons.OK);
                    return;

                }
                else if (temp.Trim().Length < 4)
                {
                    dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                    dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "กรุณากรอกรหัสร้านสาขาและกด Enter";
                    return;
                }

                foreach (DataGridViewRow row in dgvStore.Rows)
                {
                    if (row.Cells[0].Value != null && !row.ReadOnly)
                    {
                        if (row.Cells[1].Value.ToString().Substring(0, 4).Equals(temp))
                        {
                            dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                            dgvStore.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "กรุณากรอกรหัสร้านสาขาและกด Enter";
                            ShowMessageBox(true, "ไม่สามารถเพิ่มร้านสาขาที่ยังไม่ได้ทำการเขียนได้", "ข้อผิดพลาด", MessageBoxButtons.OK);
                            return;
                        }
                    }
                }

                string pathForSearch = pathForSearchAccountMonth + "\\" + selectedYearS
                    + selectedMonthS + "\\" + temp;
                Console.WriteLine(pathForSearch);
                if (Directory.Exists(pathForSearch))
                {
                    headerCheckbox.Checked = false;
                    dgvStore.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    SetRowReadOnlyProperty(dgvStore.Rows[e.RowIndex], false, false, true, true, true);
                    dgvStore.Rows[e.RowIndex].Cells[0].Value = false;
                    dgvStore.Rows[e.RowIndex].Cells[1].Value = temp + " - " + fullStoreName;
                    dgvStore.Rows[e.RowIndex].Cells[3].Value = "รอการดำเนินการ";
                    Image tempImage = (Image)imageList.Images[0];
                    tempImage.Tag = "Center";
                    ((TextAndImageCell)dgvStore.Rows[e.RowIndex].Cells[2]).Image = tempImage;
                    DataGridViewRow tempRow = (DataGridViewRow)dgvStore.RowTemplate.Clone();
                    tempRow.CreateCells(dgvStore, null, "กรุณากรอกรหัสร้านสาขาและกด Enter", "", "", "1");
                    dgvStore.Rows.Add(tempRow);
                    tempRow.Cells[1].Style.ForeColor = Color.Gray;
                    SetRowReadOnlyProperty(tempRow, false, true, false, true, true);
                }
                else
                {
                    dgvStore.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgvStore.Rows[e.RowIndex].Cells[0].Value = false;
                    dgvStore.Rows[e.RowIndex].Cells[1].Value = temp + " - " + fullStoreName;
                    dgvStore.Rows[e.RowIndex].Cells[3].Value = "รอการดำเนินการ";
                    SetRowReadOnlyProperty(dgvStore.Rows[e.RowIndex], true, true, true, true, true);
                    DataGridViewRow tempRow = (DataGridViewRow)dgvStore.RowTemplate.Clone();
                    tempRow.CreateCells(dgvStore, null, "กรุณากรอกรหัสร้านสาขาและกด Enter", "", "", "1");
                    dgvStore.Rows.Add(tempRow);
                    tempRow.Cells[1].Style.ForeColor = Color.Gray;
                    SetRowReadOnlyProperty(tempRow, false, true, false, true, true);
                }
            }
        }

        private void dgvStore_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                foreach (DataGridViewRow row in dgvStore.Rows)
                {
                    if (row.Cells[0].Value == null || row.ReadOnly)
                    {
                        continue;
                    }

                    if (!Convert.ToBoolean(row.Cells[0].EditedFormattedValue))
                    {
                        headerCheckbox.Checked = false;
                        return;
                    }
                }
                headerCheckbox.Checked = true;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DAOHelper.Instance.InsertLog(computerName, Log_Type.Process, "== END PROGRAM ==", lineCode++, 0);
            discDeviceWatcher.EndWatcher();

            if (pathForSearchAccountMonth.Length == 2)
            {
                if (Directory.Exists(pathForSearchAccountMonth))
                {
                    string command2 = "NET USE " + pathForSearchAccountMonth + " /delete /y";
                    ExecuteCommand(command2, 20000);
                }
            }

        }

        #endregion

        #region Burn Process

        private async void BurnAsync(string month, string year, List<DataGridViewRow> selectedStore)
        {
            int count = selectedStore.Count;
            int i = 1;
            foreach (DataGridViewRow row in selectedStore)
            {
                if (isStart)
                {
                    if (PreBurnProcess())
                    {
                        string sourcePath = GetTargetPath(month, year, row);

                        DateTime firstDateTime = DateTime.Now;
                        string result = await Task.Run(() => BurnProcess(sourcePath, row));
                        DateTime secondDateTime = DateTime.Now;

                        if (!result.Equals(string.Empty))
                        {
                            ShowMessageBox(true, result, "ข้อผิดพลาด", MessageBoxButtons.OK);
                        }
                        else
                        {
                            if (i == count)
                            {
                                PostEachBurn(firstDateTime, secondDateTime, row, true);
                            }
                            else
                            {
                                bool isContinue = PostEachBurn(firstDateTime, secondDateTime, row, false);
                                if (!isContinue)
                                {
                                    break;
                                }
                            }
                        }

                        if (i == count)
                        {
                            PostBurnProcess();
                        }

                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

            }
            EnableUI();
        }

        private bool PreBurnProcess()
        {
            while (true)
            {
                //if (!isDiscInserted)
                if(!DiscDeviceWatcher.isDiscInserted)
                {
                    dialogForm.SetMessagePre();
                    WriteCDModule.Instance.EjectMedia();
                    DialogResult dialogResult = dialogForm.ShowDialog(this);
                    if (dialogResult == DialogResult.Abort)
                    {
                        return false;
                    }
                    //ShowDialogForm();
                }

                string returnError = WriteCDModule.Instance.DetectMedia();
                if (string.Empty.Equals(returnError))
                {
                    return true;
                }
                else
                {
                    DialogResult dialogResult =
                        ShowMessageBox(true, returnError + " โปรดใส่แผ่นซีดีอันใหม่", "ข้อผิดพลาด",
                            MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }

        private string BurnProcess(string sourcePath, DataGridViewRow row)
        {
            SetRowState(row, "2");

            //if (settingBean.testFlag == 1)
            //{
            //    Console.WriteLine("start sleep");
            //    Thread.Sleep(2000);
            //    SetRowState(row, "1");
            //    Console.WriteLine("end sleep");
            //    AddDgvWroteRow(row);
            //    return string.Empty;
            //}
            //else if (settingBean.testFlag == 0)
            //{
            Tuple<string, Int64> receiveValue = WriteCDModule.Instance.DetectSpace();
            if (receiveValue.Item1.Equals(string.Empty))
            {
                if ((receiveValue.Item2 / 1000000) > 100)
                {
                    //Console.WriteLine(receiveValue.Item2);
                    DateTime forDateTime = new DateTime(selectedYear, selectedMonth, 1);
                    string forCDName = fileNameUtil.GetfilereportName(CDName, sourcePath.Substring(sourcePath.Length - 4, 4), forDateTime);
                    //Console.WriteLine(forCDName);
                    string returnString = WriteCDModule.Instance.BurnCD(forCDName, sourcePath);
                    if (string.Empty.Equals(returnString))
                    {
                        SetRowState(row, "1");
                        AddDgvWroteRow(row);
                        return string.Empty;
                    }
                    else
                    {
                        //Console.WriteLine(returnString);
                        SetRowState(row, "3");
                        WriteCDModule.Instance.EjectMedia();
                        return "เกิดข้อผิดพลาดขณะการเขียนแผ่นซีดี หรือมีการยกเลิกการเขียนแผ่นซีดี";
                    }
                }
                else
                {
                    SetRowState(row, "3");
                    WriteCDModule.Instance.EjectMedia();
                    return "พื้นที่ในแผ่นซีดีไม่เพียงพอ";
                }
            }
            else
            {
                SetRowState(row, "3");
                WriteCDModule.Instance.EjectMedia();
                return "เกิดข้อผิดพลาดในการตรวจสอบข้อมูลในแผ่นซีดี";
            }
            //}
            //else
            //{
            //    return "";
            //}
        }

        private bool PostEachBurn(DateTime firstDateTime, DateTime secondDateTime, DataGridViewRow row, bool isLast)
        {
            DAOHelper.Instance.InsertWroteStoreData(selectedYear, selectedMonth, row.Cells[1].Value.ToString().Substring(0, 4), computerName,
                firstDateTime, secondDateTime);

            if (isLast)
            {
                dialogForm.SetMessagePostLast(firstDateTime, secondDateTime, row.Cells[1].Value.ToString());
                dialogForm.ShowDialog(this);
                return true;
            }
            else
            {
                dialogForm.SetMessagePost(firstDateTime, secondDateTime, row.Cells[1].Value.ToString());
                while (true)
                {
                    //if (!isDiscInserted)
                    if(!DiscDeviceWatcher.isDiscInserted)
                    {
                        DialogResult dialogResult = dialogForm.ShowDialog(this);
                        if (dialogResult == DialogResult.Abort)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        private void PostBurnProcess()
        {
            ShowMessageBox(true, "กระบวนการเขียนแผ่นซีดีเสร็จสิ้น", "ข้อมูล", MessageBoxButtons.OK);
            DAOHelper.Instance.InsertLog(computerName, Log_Type.Disc, "== END BURN CD ==", lineCode++, 0);
        }

        #endregion

        private void SetRowState(DataGridViewRow row, string state)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                switch (state)
                {
                    case "1":
                        ((TextAndImageCell)row.Cells[3]).Image = (Image)imageList.Images[0];
                        row.Cells[0].Value = false;
                        row.Cells[3].Value = "สำเร็จ";
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                        row.ReadOnly = true;
                        if (row.Cells[4].Value.ToString().Equals("1"))
                        {
                            DAOHelper.Instance.InsertLog(computerName, Log_Type.DiscExtra, "== BURN STORE CODE " + row.Cells[1].Value.ToString() + " COMPLETE ==", lineCode++, 0);
                        }
                        else
                        {
                            DAOHelper.Instance.InsertLog(computerName, Log_Type.Disc, "== BURN STORE CODE " + row.Cells[1].Value.ToString() + " COMPLETE ==", lineCode++, 0);
                        }
                        break;
                    case "2":
                        ((TextAndImageCell)row.Cells[3]).Image = (Image)imageList.Images[2];
                        row.Cells[3].Value = "กำลังดำเนินการ";
                        break;
                    case "3":
                        ((TextAndImageCell)row.Cells[3]).Image = (Image)imageList.Images[1];
                        row.Cells[3].Value = "ผิดพลาด";
                        if (row.Cells[4].Value.ToString().Equals("1"))
                        {
                            DAOHelper.Instance.InsertLog(computerName, Log_Type.DiscExtra, "== BURN STORE CODE " + row.Cells[1].Value.ToString() + " FAIL ==", lineCode++, 1);
                        }
                        else
                        {
                            DAOHelper.Instance.InsertLog(computerName, Log_Type.Disc, "== BURN STORE CODE " + row.Cells[1].Value.ToString() + " FAIL ==", lineCode++, 1);
                        }
                        break;
                    default: break;
                }
            });
        }

        private void AddDgvWroteRow(DataGridViewRow row)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                DateTime currentDT = DateTime.Now;

                DataRow newRow = (dgvWrote.DataSource as DataTable).NewRow();
                newRow[0] = row.Cells[1].Value.ToString();
                newRow[1] = computerName;
                newRow[2] = currentDT;
                (dgvWrote.DataSource as DataTable).Rows.InsertAt(newRow, 0);
                //(dgvWrote.DataSource as DataTable).DefaultView.Sort = "FinishTime DESC";
            });
        }

        private void DisableUI()
        {
            comboBoxMonth.Enabled = false;
            comboBoxYear.Enabled = false;
            buttonSearch.Enabled = false;
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            headerCheckbox.Enabled = false;
            foreach (DataGridViewRow row in dgvStore.Rows)
            {
                if (row.Cells[0].Value == null)
                {
                    row.Cells[1].ReadOnly = true;
                }
                else
                {
                    row.Cells[0].ReadOnly = true;
                }
            }
        }

        private void EnableUI()
        {
            comboBoxMonth.Enabled = true;
            comboBoxYear.Enabled = true;
            buttonSearch.Enabled = true;
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            headerCheckbox.Enabled = true;

            foreach (DataGridViewRow row in dgvStore.Rows)
            {
                if (row.Cells[0].Value == null)
                {
                    row.Cells[1].ReadOnly = false;
                }
                else if (!row.ReadOnly)
                {
                    row.Cells[0].ReadOnly = false;
                }
            }
        }

        private DialogResult ShowMessageBox(bool haveOwner, string text, string title, MessageBoxButtons buttons)
        {
            DialogResult result = DialogResult.None;
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate()
                {
                    if (haveOwner)
                    {
                        result = MessageBox.Show(this, text, title, buttons);
                    }
                    else
                    {
                        result = MessageBox.Show(text, title, buttons);
                    }
                });
            }
            else
            {
                if (haveOwner)
                {
                    result = MessageBox.Show(this, text, title, buttons);
                }
                else
                {
                    result = MessageBox.Show(text, title, buttons);
                }
            }
            return result;
        }

        /////////////////
        /////EVENT///////

        private void OnInsertDisc()
        {
            //IS_DISC_INSERTED = true;
            dialogForm.HideDialog();
        }

        private void OnEjectDisc()
        {
            //IS_DISC_INSERTED = false;
        }

        /////////////////

        private List<string> GetDirectoryList(string pathForSearch)
        {
            if (Directory.Exists(pathForSearch))
            {
                List<string> directoryList = new List<string>();
                IEnumerable<string> directoryEnum = Directory.EnumerateDirectories(pathForSearch);

                foreach (string item in directoryEnum)
                {
                    string[] split = item.Split('\\');
                    string forAdd = split[split.Length - 1];
                    directoryList.Add(forAdd);
                }
                return directoryList;
            }
            else
            {
                return null;
            }
        }

        private string GetTargetPath(string month, string year, DataGridViewRow row)
        {
            string tempSourcePath = pathForSearchAccountMonth + @"\" + year + month + @"\";
            string sourcePath = tempSourcePath + row.Cells[1].Value.ToString().Substring(0, 4);
            return sourcePath;
        }

        private void SetRowReadOnlyProperty(DataGridViewRow row, bool setReadOnly, params bool[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                row.Cells[i].ReadOnly = args[i];
            }
            if (setReadOnly)
            {
                row.ReadOnly = true;
            }
        }

        private List<string> ConvertToListString(List<DataRow> dataRowList, int column)
        {
            List<string> returnList = new List<string>();
            foreach (DataRow row in dataRowList)
            {
                returnList.Add(row[column].ToString());
            }
            return returnList;
        }

        private int ExecuteCommand(string command, int timeout)
        {
            try
            {
                var processInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    //RedirectStandardOutput = true,
                    //RedirectStandardInput = true,
                    //RedirectStandardError = true,
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
