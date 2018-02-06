namespace FCWriteCD.FormInterface
{
    partial class DeviceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceForm));
            this.buttonDetectMedia = new System.Windows.Forms.Button();
            this.devicesComboBox = new System.Windows.Forms.ComboBox();
            this.labelFreeSpace = new System.Windows.Forms.Label();
            this.labelMediaType = new System.Windows.Forms.Label();
            this.labelDrive = new System.Windows.Forms.Label();
            this.labelSupportMedia = new System.Windows.Forms.Label();
            this.buttonEject = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelFreeSpaceValue = new System.Windows.Forms.Label();
            this.labelMediaTypeValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonDetectMedia
            // 
            this.buttonDetectMedia.Location = new System.Drawing.Point(15, 168);
            this.buttonDetectMedia.Name = "buttonDetectMedia";
            this.buttonDetectMedia.Size = new System.Drawing.Size(85, 33);
            this.buttonDetectMedia.TabIndex = 0;
            this.buttonDetectMedia.Text = "Detect Media";
            this.buttonDetectMedia.UseVisualStyleBackColor = true;
            this.buttonDetectMedia.Visible = false;
            // 
            // devicesComboBox
            // 
            this.devicesComboBox.FormattingEnabled = true;
            this.devicesComboBox.Location = new System.Drawing.Point(57, 39);
            this.devicesComboBox.Name = "devicesComboBox";
            this.devicesComboBox.Size = new System.Drawing.Size(253, 21);
            this.devicesComboBox.TabIndex = 1;
            this.devicesComboBox.SelectedIndexChanged += new System.EventHandler(this.devicesComboBox_SelectedIndexChanged);
            // 
            // labelFreeSpace
            // 
            this.labelFreeSpace.AutoSize = true;
            this.labelFreeSpace.Location = new System.Drawing.Point(120, 188);
            this.labelFreeSpace.Name = "labelFreeSpace";
            this.labelFreeSpace.Size = new System.Drawing.Size(68, 13);
            this.labelFreeSpace.TabIndex = 6;
            this.labelFreeSpace.Text = "Free Space :";
            this.labelFreeSpace.Visible = false;
            // 
            // labelMediaType
            // 
            this.labelMediaType.AutoSize = true;
            this.labelMediaType.Location = new System.Drawing.Point(120, 168);
            this.labelMediaType.Name = "labelMediaType";
            this.labelMediaType.Size = new System.Drawing.Size(69, 13);
            this.labelMediaType.TabIndex = 7;
            this.labelMediaType.Text = "Media Type :";
            this.labelMediaType.Visible = false;
            // 
            // labelDrive
            // 
            this.labelDrive.AutoSize = true;
            this.labelDrive.Location = new System.Drawing.Point(12, 42);
            this.labelDrive.Name = "labelDrive";
            this.labelDrive.Size = new System.Drawing.Size(32, 13);
            this.labelDrive.TabIndex = 8;
            this.labelDrive.Text = "Drive";
            // 
            // labelSupportMedia
            // 
            this.labelSupportMedia.Location = new System.Drawing.Point(12, 75);
            this.labelSupportMedia.Name = "labelSupportMedia";
            this.labelSupportMedia.Size = new System.Drawing.Size(298, 66);
            this.labelSupportMedia.TabIndex = 9;
            this.labelSupportMedia.Text = "Support Profile";
            // 
            // buttonEject
            // 
            this.buttonEject.Location = new System.Drawing.Point(15, 219);
            this.buttonEject.Name = "buttonEject";
            this.buttonEject.Size = new System.Drawing.Size(85, 33);
            this.buttonEject.TabIndex = 10;
            this.buttonEject.Text = "Eject";
            this.buttonEject.UseVisualStyleBackColor = true;
            this.buttonEject.Visible = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(225, 219);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(85, 33);
            this.buttonOK.TabIndex = 11;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelFreeSpaceValue
            // 
            this.labelFreeSpaceValue.AutoSize = true;
            this.labelFreeSpaceValue.Location = new System.Drawing.Point(195, 188);
            this.labelFreeSpaceValue.Name = "labelFreeSpaceValue";
            this.labelFreeSpaceValue.Size = new System.Drawing.Size(45, 13);
            this.labelFreeSpaceValue.TabIndex = 12;
            this.labelFreeSpaceValue.Text = "No Disc";
            this.labelFreeSpaceValue.Visible = false;
            // 
            // labelMediaTypeValue
            // 
            this.labelMediaTypeValue.AutoSize = true;
            this.labelMediaTypeValue.Location = new System.Drawing.Point(195, 168);
            this.labelMediaTypeValue.Name = "labelMediaTypeValue";
            this.labelMediaTypeValue.Size = new System.Drawing.Size(115, 13);
            this.labelMediaTypeValue.TabIndex = 13;
            this.labelMediaTypeValue.Text = "No Disc or Disc Invalid";
            this.labelMediaTypeValue.Visible = false;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 270);
            this.Controls.Add(this.labelMediaTypeValue);
            this.Controls.Add(this.labelFreeSpaceValue);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonEject);
            this.Controls.Add(this.labelSupportMedia);
            this.Controls.Add(this.labelDrive);
            this.Controls.Add(this.labelMediaType);
            this.Controls.Add(this.labelFreeSpace);
            this.Controls.Add(this.devicesComboBox);
            this.Controls.Add(this.buttonDetectMedia);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StartForm";
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDetectMedia;
        private System.Windows.Forms.ComboBox devicesComboBox;
        private System.Windows.Forms.Label labelFreeSpace;
        private System.Windows.Forms.Label labelMediaType;
        private System.Windows.Forms.Label labelDrive;
        private System.Windows.Forms.Label labelSupportMedia;
        private System.Windows.Forms.Button buttonEject;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelFreeSpaceValue;
        private System.Windows.Forms.Label labelMediaTypeValue;
    }
}

