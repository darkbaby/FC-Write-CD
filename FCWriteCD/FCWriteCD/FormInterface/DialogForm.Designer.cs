namespace FCWriteCD.FormInterface
{
    partial class DialogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogForm));
            this.labelMessage1 = new System.Windows.Forms.Label();
            this.labelMessage4 = new System.Windows.Forms.Label();
            this.labelMessage5 = new System.Windows.Forms.Label();
            this.labelMessage6 = new System.Windows.Forms.Label();
            this.labelMessage2 = new System.Windows.Forms.Label();
            this.labelMessage3 = new System.Windows.Forms.Label();
            this.labelMessage7 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelMessage8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelMessage1
            // 
            this.labelMessage1.AutoSize = true;
            this.labelMessage1.Location = new System.Drawing.Point(12, 9);
            this.labelMessage1.Name = "labelMessage1";
            this.labelMessage1.Size = new System.Drawing.Size(246, 13);
            this.labelMessage1.TabIndex = 1;
            this.labelMessage1.Text = "การเขียนข้อมูลลงแผ่น CD ของร้านสาขา {0} เสร็จสิ้น";
            // 
            // labelMessage4
            // 
            this.labelMessage4.AutoSize = true;
            this.labelMessage4.Location = new System.Drawing.Point(12, 101);
            this.labelMessage4.Name = "labelMessage4";
            this.labelMessage4.Size = new System.Drawing.Size(134, 13);
            this.labelMessage4.TabIndex = 2;
            this.labelMessage4.Text = "กรุณาทำตามขั้นตอนต่อไปนี้";
            // 
            // labelMessage5
            // 
            this.labelMessage5.AutoSize = true;
            this.labelMessage5.Location = new System.Drawing.Point(12, 126);
            this.labelMessage5.Name = "labelMessage5";
            this.labelMessage5.Size = new System.Drawing.Size(340, 13);
            this.labelMessage5.TabIndex = 3;
            this.labelMessage5.Text = "1. นำแผ่น CD ออกจากเครื่อง และ เขียนรหัสร้านสาขา {0} ลงบนแผ่น CD";
            // 
            // labelMessage6
            // 
            this.labelMessage6.AutoSize = true;
            this.labelMessage6.Location = new System.Drawing.Point(12, 155);
            this.labelMessage6.Name = "labelMessage6";
            this.labelMessage6.Size = new System.Drawing.Size(319, 13);
            this.labelMessage6.TabIndex = 4;
            this.labelMessage6.Text = "2. นำแผ่น CD ใหม่ใส่ลงในเครื่องอ่านแผ่น CD และปิดถาดอ่านแผ่น";
            // 
            // labelMessage2
            // 
            this.labelMessage2.AutoSize = true;
            this.labelMessage2.Location = new System.Drawing.Point(12, 37);
            this.labelMessage2.Name = "labelMessage2";
            this.labelMessage2.Size = new System.Drawing.Size(71, 13);
            this.labelMessage2.TabIndex = 5;
            this.labelMessage2.Text = "เวลาเริ่มต้น : ";
            // 
            // labelMessage3
            // 
            this.labelMessage3.AutoSize = true;
            this.labelMessage3.Location = new System.Drawing.Point(12, 63);
            this.labelMessage3.Name = "labelMessage3";
            this.labelMessage3.Size = new System.Drawing.Size(65, 13);
            this.labelMessage3.TabIndex = 6;
            this.labelMessage3.Text = "เวลาสิ้นสุด : ";
            // 
            // labelMessage7
            // 
            this.labelMessage7.AutoSize = true;
            this.labelMessage7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.labelMessage7.Location = new System.Drawing.Point(164, 83);
            this.labelMessage7.Name = "labelMessage7";
            this.labelMessage7.Size = new System.Drawing.Size(188, 31);
            this.labelMessage7.TabIndex = 7;
            this.labelMessage7.Text = "กรุณาใส่แผ่นซีดี";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(434, 12);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.panel1.Controls.Add(this.labelMessage8);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Location = new System.Drawing.Point(1, 186);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 46);
            this.panel1.TabIndex = 9;
            // 
            // labelMessage8
            // 
            this.labelMessage8.AutoSize = true;
            this.labelMessage8.Location = new System.Drawing.Point(11, 17);
            this.labelMessage8.Name = "labelMessage8";
            this.labelMessage8.Size = new System.Drawing.Size(281, 13);
            this.labelMessage8.TabIndex = 10;
            this.labelMessage8.Text = "*กดปุ่มกากบาทถ้าต้องการยกเลิกกระบวนการเขียนแผ่นซีดี*";
            // 
            // DialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 232);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelMessage7);
            this.Controls.Add(this.labelMessage3);
            this.Controls.Add(this.labelMessage2);
            this.Controls.Add(this.labelMessage6);
            this.Controls.Add(this.labelMessage5);
            this.Controls.Add(this.labelMessage4);
            this.Controls.Add(this.labelMessage1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DialogForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMessage1;
        private System.Windows.Forms.Label labelMessage4;
        private System.Windows.Forms.Label labelMessage5;
        private System.Windows.Forms.Label labelMessage6;
        private System.Windows.Forms.Label labelMessage2;
        private System.Windows.Forms.Label labelMessage3;
        private System.Windows.Forms.Label labelMessage7;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelMessage8;
    }
}