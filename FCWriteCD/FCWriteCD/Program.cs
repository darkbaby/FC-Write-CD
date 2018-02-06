using System;
using System.Windows.Forms;
using FCWriteCD.FormInterface;

namespace FCWriteCD
{
    public static class Program
    {
        public static DeviceForm startForm;
        public static MainForm mainForm;
        public static bool isContinueMainForm = false;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LandingForm landingForm = new LandingForm();
            Application.Run(landingForm);

            if (isContinueMainForm)
            {
                Application.Run(mainForm);
            }
            else
            {
                return;
            }
        }
    }
}
