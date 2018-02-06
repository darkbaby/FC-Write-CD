namespace FCWriteCD.FormInterface
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBoxACM = new System.Windows.Forms.GroupBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.labelYear = new System.Windows.Forms.Label();
            this.comboBoxMonth = new System.Windows.Forms.ComboBox();
            this.labelMonth = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.headerCheckbox = new System.Windows.Forms.CheckBox();
            this.dgvStore = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.labelFilter = new System.Windows.Forms.Label();
            this.comboBoxFilter = new System.Windows.Forms.ComboBox();
            this.dgvWrote = new System.Windows.Forms.DataGridView();
            this.textAndImageColumn1 = new FCWriteCD.CustomColumn.TextAndImageColumn();
            this.textAndImageColumn2 = new FCWriteCD.CustomColumn.TextAndImageColumn();
            this.SelectCheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StoreName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.haveFolder = new FCWriteCD.CustomColumn.TextAndImageColumn();
            this.Status = new FCWriteCD.CustomColumn.TextAndImageColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StoreNameWrote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComputerNameWrote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FinishTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxACM.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStore)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWrote)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxACM
            // 
            this.groupBoxACM.Controls.Add(this.buttonSearch);
            this.groupBoxACM.Controls.Add(this.comboBoxYear);
            this.groupBoxACM.Controls.Add(this.labelYear);
            this.groupBoxACM.Controls.Add(this.comboBoxMonth);
            this.groupBoxACM.Controls.Add(this.labelMonth);
            this.groupBoxACM.Location = new System.Drawing.Point(47, 25);
            this.groupBoxACM.Name = "groupBoxACM";
            this.groupBoxACM.Size = new System.Drawing.Size(465, 55);
            this.groupBoxACM.TabIndex = 0;
            this.groupBoxACM.TabStop = false;
            this.groupBoxACM.Text = "Account Month";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(380, 21);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(79, 23);
            this.buttonSearch.TabIndex = 1;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxYear.FormattingEnabled = true;
            this.comboBoxYear.Location = new System.Drawing.Point(222, 23);
            this.comboBoxYear.Name = "comboBoxYear";
            this.comboBoxYear.Size = new System.Drawing.Size(121, 21);
            this.comboBoxYear.TabIndex = 3;
            this.comboBoxYear.SelectedIndexChanged += new System.EventHandler(this.comboBoxYear_SelectedIndexChanged);
            // 
            // labelYear
            // 
            this.labelYear.AutoSize = true;
            this.labelYear.Location = new System.Drawing.Point(201, 26);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(14, 13);
            this.labelYear.TabIndex = 100;
            this.labelYear.Text = "ปี";
            // 
            // comboBoxMonth
            // 
            this.comboBoxMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonth.FormattingEnabled = true;
            this.comboBoxMonth.Location = new System.Drawing.Point(56, 23);
            this.comboBoxMonth.Name = "comboBoxMonth";
            this.comboBoxMonth.Size = new System.Drawing.Size(121, 21);
            this.comboBoxMonth.TabIndex = 2;
            this.comboBoxMonth.SelectedIndexChanged += new System.EventHandler(this.comboBoxMonth_SelectedIndexChanged);
            // 
            // labelMonth
            // 
            this.labelMonth.AutoSize = true;
            this.labelMonth.Location = new System.Drawing.Point(15, 26);
            this.labelMonth.Name = "labelMonth";
            this.labelMonth.Size = new System.Drawing.Size(33, 13);
            this.labelMonth.TabIndex = 100;
            this.labelMonth.Text = "เดือน";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(571, 48);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(662, 48);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 5;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.headerCheckbox);
            this.groupBox1.Controls.Add(this.dgvStore);
            this.groupBox1.Location = new System.Drawing.Point(47, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(690, 158);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ร้านสาขาที่ยังไม่ได้ Write";
            // 
            // headerCheckbox
            // 
            this.headerCheckbox.AutoSize = true;
            this.headerCheckbox.Enabled = false;
            this.headerCheckbox.Location = new System.Drawing.Point(52, 24);
            this.headerCheckbox.Name = "headerCheckbox";
            this.headerCheckbox.Size = new System.Drawing.Size(15, 14);
            this.headerCheckbox.TabIndex = 7;
            this.headerCheckbox.UseVisualStyleBackColor = true;
            this.headerCheckbox.Click += new System.EventHandler(this.headerCheckbox_Click);
            // 
            // dgvStore
            // 
            this.dgvStore.AllowUserToAddRows = false;
            this.dgvStore.AllowUserToDeleteRows = false;
            this.dgvStore.AllowUserToResizeColumns = false;
            this.dgvStore.AllowUserToResizeRows = false;
            this.dgvStore.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStore.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectCheckBox,
            this.StoreName,
            this.haveFolder,
            this.Status,
            this.State});
            this.dgvStore.Location = new System.Drawing.Point(6, 19);
            this.dgvStore.MultiSelect = false;
            this.dgvStore.Name = "dgvStore";
            this.dgvStore.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvStore.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStore.Size = new System.Drawing.Size(678, 133);
            this.dgvStore.TabIndex = 100;
            this.dgvStore.TabStop = false;
            this.dgvStore.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvStore_CellBeginEdit);
            this.dgvStore.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStore_CellContentClick);
            this.dgvStore.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStore_CellEndEdit);
            this.dgvStore.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvStore_EditingControlShowing);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonRefresh);
            this.groupBox2.Controls.Add(this.labelFilter);
            this.groupBox2.Controls.Add(this.comboBoxFilter);
            this.groupBox2.Controls.Add(this.dgvWrote);
            this.groupBox2.Location = new System.Drawing.Point(47, 250);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(690, 157);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ร้านสาขาที่ Write แล้ว";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Enabled = false;
            this.buttonRefresh.Location = new System.Drawing.Point(192, 16);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(54, 23);
            this.buttonRefresh.TabIndex = 10;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(15, 21);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(40, 13);
            this.labelFilter.TabIndex = 6;
            this.labelFilter.Text = "ชื่อคอม";
            // 
            // comboBoxFilter
            // 
            this.comboBoxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilter.Enabled = false;
            this.comboBoxFilter.FormattingEnabled = true;
            this.comboBoxFilter.Location = new System.Drawing.Point(56, 17);
            this.comboBoxFilter.Name = "comboBoxFilter";
            this.comboBoxFilter.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFilter.TabIndex = 9;
            this.comboBoxFilter.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // dgvWrote
            // 
            this.dgvWrote.AllowUserToAddRows = false;
            this.dgvWrote.AllowUserToDeleteRows = false;
            this.dgvWrote.AllowUserToResizeColumns = false;
            this.dgvWrote.AllowUserToResizeRows = false;
            this.dgvWrote.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWrote.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StoreNameWrote,
            this.ComputerNameWrote,
            this.FinishTime});
            this.dgvWrote.Location = new System.Drawing.Point(6, 45);
            this.dgvWrote.MultiSelect = false;
            this.dgvWrote.Name = "dgvWrote";
            this.dgvWrote.ReadOnly = true;
            this.dgvWrote.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvWrote.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWrote.Size = new System.Drawing.Size(678, 106);
            this.dgvWrote.TabIndex = 100;
            this.dgvWrote.TabStop = false;
            // 
            // textAndImageColumn1
            // 
            this.textAndImageColumn1.Image = null;
            this.textAndImageColumn1.Name = "textAndImageColumn1";
            // 
            // textAndImageColumn2
            // 
            this.textAndImageColumn2.Image = null;
            this.textAndImageColumn2.Name = "textAndImageColumn2";
            // 
            // SelectCheckBox
            // 
            this.SelectCheckBox.FalseValue = "false";
            this.SelectCheckBox.HeaderText = "    เลือก";
            this.SelectCheckBox.Name = "SelectCheckBox";
            this.SelectCheckBox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SelectCheckBox.TrueValue = "true";
            this.SelectCheckBox.Width = 50;
            // 
            // StoreName
            // 
            this.StoreName.HeaderText = "ร้านสาขา";
            this.StoreName.Name = "StoreName";
            this.StoreName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.StoreName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StoreName.Width = 315;
            // 
            // haveFolder
            // 
            this.haveFolder.HeaderText = "สถานะไฟล์ใน Folder";
            this.haveFolder.Image = null;
            this.haveFolder.Name = "haveFolder";
            this.haveFolder.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.haveFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.haveFolder.Width = 110;
            // 
            // Status
            // 
            this.Status.HeaderText = "สถานะ";
            this.Status.Image = null;
            this.Status.Name = "Status";
            this.Status.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Status.Width = 125;
            // 
            // State
            // 
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.Visible = false;
            // 
            // StoreNameWrote
            // 
            this.StoreNameWrote.DataPropertyName = "StoreNameWrote";
            this.StoreNameWrote.HeaderText = "ร้านสาขา";
            this.StoreNameWrote.Name = "StoreNameWrote";
            this.StoreNameWrote.ReadOnly = true;
            this.StoreNameWrote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StoreNameWrote.Width = 315;
            // 
            // ComputerNameWrote
            // 
            this.ComputerNameWrote.DataPropertyName = "ComputerNameWrote";
            this.ComputerNameWrote.HeaderText = "ชื่อคอมที่ Write";
            this.ComputerNameWrote.Name = "ComputerNameWrote";
            this.ComputerNameWrote.ReadOnly = true;
            this.ComputerNameWrote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ComputerNameWrote.Width = 160;
            // 
            // FinishTime
            // 
            this.FinishTime.DataPropertyName = "FinishTime";
            dataGridViewCellStyle1.Format = "dd/MM/yyyy HH:mm:ss";
            this.FinishTime.DefaultCellStyle = dataGridViewCellStyle1;
            this.FinishTime.HeaderText = "เวลาที่แล้วเสร็จ";
            this.FinishTime.Name = "FinishTime";
            this.FinishTime.ReadOnly = true;
            this.FinishTime.Width = 125;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 419);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.groupBoxACM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FC Write CD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBoxACM.ResumeLayout(false);
            this.groupBoxACM.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStore)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWrote)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxACM;
        private System.Windows.Forms.ComboBox comboBoxYear;
        private System.Windows.Forms.Label labelYear;
        private System.Windows.Forms.ComboBox comboBoxMonth;
        private System.Windows.Forms.Label labelMonth;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvStore;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvWrote;
        private System.Windows.Forms.CheckBox headerCheckbox;
        private CustomColumn.TextAndImageColumn textAndImageColumn1;
        private CustomColumn.TextAndImageColumn textAndImageColumn2;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.ComboBox comboBoxFilter;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn StoreName;
        private CustomColumn.TextAndImageColumn haveFolder;
        private CustomColumn.TextAndImageColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn StoreNameWrote;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComputerNameWrote;
        private System.Windows.Forms.DataGridViewTextBoxColumn FinishTime;
    }
}