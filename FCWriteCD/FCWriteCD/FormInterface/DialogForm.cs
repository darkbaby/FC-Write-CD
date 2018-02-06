using FCWriteCD.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCWriteCD.FormInterface
{
    public partial class DialogForm : Form
    {
        private MainForm mainForm;

        private bool isLastWork;

        public DialogForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            Initialize();
        }

        private void Initialize()
        {
            isLastWork = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        public void SetMessageWait()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate()
                {
                    labelMessage1.Visible = false;
                    labelMessage2.Visible = false;
                    labelMessage3.Visible = false;
                    labelMessage4.Visible = false;
                    labelMessage5.Visible = false;
                    labelMessage6.Visible = false;
                    labelMessage7.Visible = true;
                    buttonOK.Visible = false;
                    this.Text = "แจ้งเตือน";
                    labelMessage7.Text = "กำลังเตรียมเปิดโปรแกรม";
                });
            }
            else
            {
                labelMessage1.Visible = false;
                labelMessage2.Visible = false;
                labelMessage3.Visible = false;
                labelMessage4.Visible = false;
                labelMessage5.Visible = false;
                labelMessage6.Visible = false;
                labelMessage7.Visible = true;
                buttonOK.Visible = false;
                this.Text = "แจ้งเตือน";
                labelMessage7.Text = "กำลังเตรียมเปิดโปรแกรม";
            }
        }

        public void SetMessagePre()
        {
            labelMessage1.Visible = false;
            labelMessage2.Visible = false;
            labelMessage3.Visible = false;
            labelMessage4.Visible = false;
            labelMessage5.Visible = false;
            labelMessage6.Visible = false;
            labelMessage7.Visible = true;
            labelMessage8.Visible = false;
            buttonOK.Visible = true;

            labelMessage7.Text = "กรุณาใส่แผ่นซีดี";

            this.Text = "แจ้งเตือน";
        }

        public void SetMessagePost(DateTime firstDateTime, DateTime secondDateTime, string storeName)
        {
            isLastWork = false;

            labelMessage1.Visible = true;
            labelMessage2.Visible = true;
            labelMessage3.Visible = true;
            labelMessage4.Visible = true;
            labelMessage5.Visible = true;
            labelMessage6.Visible = true;
            labelMessage7.Visible = false;
            labelMessage8.Visible = true;
            buttonOK.Visible = true;

            this.Text = "การเขียนข้อมูลลงแผ่น CD เสร็จสิ้น";

            string text1 = "การเขียนข้อมูลลงแผ่น CD ของร้านสาขา {0} เสร็จสิ้น";
            labelMessage1.Text = string.Format(text1, storeName);

            labelMessage2.Text = "เวลาเริ่มต้น : " + firstDateTime.ToString();
            labelMessage3.Text = "เวลาสิ้นสุด : " + secondDateTime.ToString();

            string text5 = "1. นำแผ่น CD ออกจากเครื่อง และ เขียนรหัสร้านสาขา {0} ลงบนแผ่น CD";
            labelMessage5.Text = string.Format(text5, storeName.Substring(0,4));
        }

        public void SetMessagePostLast(DateTime firstDateTime, DateTime secondDateTime, string storeName)
        {

            isLastWork = true;

            labelMessage1.Visible = true;
            labelMessage2.Visible = true;
            labelMessage3.Visible = true;
            labelMessage4.Visible = true;
            labelMessage5.Visible = true;
            labelMessage6.Visible = false;
            labelMessage7.Visible = false;
            labelMessage8.Visible = false;
            buttonOK.Visible = true;

            this.Text = "การเขียนข้อมูลลงแผ่น CD เสร็จสิ้น";

            string text1 = "การเขียนข้อมูลลงแผ่น CD ของร้านสาขา {0} เสร็จสิ้น";
            labelMessage1.Text = string.Format(text1, storeName);

            labelMessage2.Text = "เวลาเริ่มต้น : " + firstDateTime.ToString();
            labelMessage3.Text = "เวลาสิ้นสุด : " + secondDateTime.ToString();

            string text5 = "1. นำแผ่น CD ออกจากเครื่อง และ เขียนรหัสร้านสาขา {0} ลงบนแผ่น CD";
            labelMessage5.Text = string.Format(text5, storeName.Substring(0, 4));

        }

        public void HideDialog()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    this.Hide();
                });
            }
            else
            {
                this.Hide();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (DiscDeviceWatcher.isDiscInserted || isLastWork)
            {
                this.HideDialog();
            }
        }

        private void DialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult = DialogResult = DialogResult.Abort;
            }
        }
    }
}
