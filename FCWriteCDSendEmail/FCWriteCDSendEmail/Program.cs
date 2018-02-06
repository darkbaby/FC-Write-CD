using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Data;
using System.Threading;

namespace FCWriteCDSendEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime currentDT = DateTime.Now;
            int yearTarget = currentDT.Year;
            int monthTarget = currentDT.Month - 1;
            if (monthTarget == 0)
            {
                yearTarget = yearTarget - 1;
                monthTarget = 12;
            }
            
            ////
            yearTarget = 2016;
            monthTarget = 10;
            ////



            //DatabaseManagement databaseManagement = new DatabaseManagement(@"192.168.10.192\SQL2008", "FC_WRITE_CD",
            //    "sa", "P@ssw0rd", 15);

            DatabaseManagement databaseManagement = new DatabaseManagement(@"10.35.0.150", "ACC_FC_WRITE_CD",
                "sa", "P@ssw0rd", 15);
            DataTable dataSource = databaseManagement.GetAllComputerMapStore(yearTarget, monthTarget);

            if (dataSource.Rows.Count < 1)
            {
                return;
            }

            Dictionary<string, DataTable> dataSourceAdjusted = databaseManagement.AdjustComputerMapStore(dataSource, yearTarget, monthTarget);

            Console.WriteLine("RECEIVED DATA");

            //ExcelManagament excelManagement = new ExcelManagament("WriteCDStoreList_" + yearTarget.ToString() + monthTarget.ToString().PadLeft(2, '0'));
            //excelManagement.CreateExcelFile();
            //excelManagement.AddSheet(dataSourceAdjusted);
            //foreach (KeyValuePair<string, DataTable> each in dataSourceAdjusted)
            //{
            //    excelManagement.AddSheet(each.Key, each.Value);
            //}
            //excelManagement.ReleaseAllReference();

            DirectoryManagement directoryManagement = new DirectoryManagement();
            directoryManagement.CreateTempDirectory();

            string fileName = @"C:\fc_write_cd_temp\WriteCDStoreList_" + yearTarget + "" + monthTarget.ToString().PadLeft(2,'0') + ".xls";

            ExcelManagement2 excelManagement2 = new ExcelManagement2();
            excelManagement2.CreateExcelFile(dataSourceAdjusted, fileName);
            Console.WriteLine("EXPORT EXCEL COMPLETE");

            //SmtpClient client = new SmtpClient();
            //client.Port = 25;
            //client.Host = "10.31.15.30";
            ////client.EnableSsl = true;
            //client.Timeout = 30000;
            ////client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            ////client.Credentials = new System.Net.NetworkCredential("chotirote.k.esynergy@gmail.com", "sakurachat");

            //MailMessage mm = new MailMessage("FC_SEND_EMAIL@esynergy-corp.com", "chotirote.k.esynergy@gmail.com");
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            //mm.Body = "fsdfsdfdsfdsfdsf cho ja";
            ////Attachment attachment = new Attachment(@"D:\WriteCDStoreList_201609.xls");
            ////mm.Attachments.Add(attachment);
            //client.Send(mm);

            databaseManagement.SendEmail(fileName);

            Console.WriteLine("SEND EMAIL COMPLETE");

            directoryManagement.DeleteTempDirectory();

            Console.WriteLine("PROGRAM WILL AUTOMATICALLY CLOSE");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }
    }
}
