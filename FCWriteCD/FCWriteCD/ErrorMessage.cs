using FCWriteCD.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCWriteCD
{
    class ErrorMessage
    {

        public ErrorMessage()
        {

        }

        public void Show(IWin32Window window ,int errorCode)
        {
            switch (errorCode)
            {
                case 1:
                    MessageBox.Show(window, "ไม่สามารถเชื่อมต่อเซิฟเวอร์ที่กำหนดได้ กรุณาตรวจสอบไฟล์ setting.ini", "แจ้งเตือน", MessageBoxButtons.OK);
                    break;
                case 2:
                    DAOHelper.Instance.InsertLog(Environment.MachineName, Log_Type.Directory, "== YOUR COMPUTER DONT HAVE PERMISSION, CANT START PROGRAM ==", 0, 1);
                    MessageBox.Show(window, "คอมพิวเตอร์ของคุณไม่ได้รับอนุญาตให้ใช้งานโปรแกรม", "แจ้งเตือน", MessageBoxButtons.OK);
                    break;
                case 3:
                    DAOHelper.Instance.InsertLog(Environment.MachineName, Log_Type.Directory, "== DIDNT FIND THE GIVEN TARGET PATH, CANT START PROGRAM ==", 0, 1);
                    MessageBox.Show(window, "ไม่พบโฟลเดอร์ปลายทางที่กำหนดไว้ในฐานข้อมูล", "เแจ้งเตือน", MessageBoxButtons.OK);
                    break;
                case 4:
                    DAOHelper.Instance.InsertLog(Environment.MachineName, Log_Type.Directory, "== USERNAME, PASSWORD ARE NOT COMPATIBLE TO MAP, OR EMPTY DRIVE IS INVALID ==", 0, 1);
                    MessageBox.Show(window, "Map Network Drive ไม่สำเร็จ กรุณาตรวจสอบข้อมูลที่กำหนดไว้ในฐานข้อมูลใหม่", "เแจ้งเตือน", MessageBoxButtons.OK);
                    break;
                case 5:
                    DAOHelper.Instance.InsertLog(Environment.MachineName, Log_Type.Directory, "== TARGET FOLDER IS INVALID, CANT START PROGRAM ==", 0, 1);
                    MessageBox.Show(window, "โฟลเดอร์ปลายทางไม่ได้เก็บ Account Month กรุณาเปลี่ยนโฟลเดอร์ปลายทางใหม่ (YYYYMM)", "เแจ้งเตือน", MessageBoxButtons.OK);
                    break;
                default:
                    break;
            }
        }
    }
}
