using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace FCWriteCDSendEmail
{
    class ExcelManagament
    {
        private Application excelApp;
        private Workbook excelWorkbook;
        private Workbooks excelWorkbooks;
        private Sheets excelWorksheets;

        private string pathToSave = @"D:\";
        private string fileName;

        public ExcelManagament(string name)
        {
            excelApp = null;
            excelWorkbook = null;
            excelWorkbooks = null;
            excelWorksheets = null;
            fileName = name;
        }

        public string GetFilePath()
        {
            return pathToSave + fileName + ".xls";
        }

        public void CreateExcelFile()
        {
            excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.DisplayAlerts = false;
            excelApp.ScreenUpdating = false;
            excelApp.Visible = false;

            excelWorkbooks = excelApp.Workbooks;
            excelWorkbook = excelWorkbooks.Add("");

            //Worksheet excelWorksheet = (Worksheet)excelWorkbook.ActiveSheet;

            //Range excelRange = excelWorksheet.get_Range("A1", "B200");
            //excelRange.NumberFormat = "@";
            //excelRange.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            //for (int i = 0; i < 200; i++)
            //{
            //    for (int j = 0; j < 2; j++)
            //    {
            //        excelWorksheet.Cells[i + 1, j + 1] = "test" + i + "" + j;
            //    }
            //}

            //excelRange.EntireColumn.AutoFit();
            excelWorkbook.SaveAs(pathToSave + fileName + ".xls", XlFileFormat.xlExcel8,
                Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlExclusive,
                XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
            excelWorkbook.Close(false);
            excelWorkbooks.Close();

            //excelApp.Quit();
            releaseObject(excelWorkbook);
            releaseObject(excelWorkbooks);
            releaseObject(excelWorksheets);
            //releaseObject(excelApp);
        }

        public void AddSheet(Dictionary<string,System.Data.DataTable> dataSource)
        {
            //excelApp = new Microsoft.Office.Interop.Excel.Application();
            //excelApp.DisplayAlerts = false;
            //excelApp.ScreenUpdating = false;
            //excelApp.Visible = false;

            excelWorkbooks = excelApp.Workbooks;
            excelWorkbook = excelWorkbooks.Open(pathToSave + fileName + ".xls", Missing.Value, false, Missing.Value, Missing.Value,
                        Missing.Value, false, Missing.Value, Missing.Value, true, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value);
            excelWorksheets = excelWorkbook.Worksheets;


            foreach (KeyValuePair<string, System.Data.DataTable> each in dataSource)
            {
                int workSheetsCount = excelWorksheets.Count;

                Worksheet excelWorksheet = (Worksheet)excelWorksheets.Add(After: excelWorksheets[workSheetsCount]);
                excelWorksheet.Name = each.Key;

                Worksheet firstWorksheet = (Worksheet)excelWorksheets[1];
                string firstWorksheetName = firstWorksheet.Name;
                if (firstWorksheetName.Equals("Sheet1"))
                {
                    firstWorksheet.Delete();
                }

                Range excelRange = excelWorksheet.get_Range("A1", "B500");
                excelRange.NumberFormat = "@";
                Range tempExcelRange = excelRange.Cells;
                tempExcelRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                //excelRange.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;


                excelWorksheet.Cells[1, 1] = "Store Code";
                excelWorksheet.Cells[1, 2] = "Store Name";
                int totalRow = each.Value.Rows.Count;
                int totalColumn = 2;
                for (int i = 1; i <= totalRow; i++)
                {
                    for (int j = 1; j <= totalColumn; j++)
                    {
                        excelWorksheet.Cells[i + 1, j] = each.Value.Rows[i - 1][j - 1].ToString();
                    }
                }
                Range tempExcelRange2 = excelRange.EntireColumn;
                tempExcelRange2.AutoFit();
                //excelRange.EntireColumn.AutoFit();
            }

            excelWorkbook.Save();
            excelWorkbook.Close(false);
            excelWorkbooks.Close();

            excelApp.Quit();
            releaseObject(excelWorkbook);
            releaseObject(excelWorkbooks);
            releaseObject(excelWorksheets);
            releaseObject(excelApp);
        }

        private void releaseObject(object obj)
        {
            try
            {
                //if (obj == null)
                //{
                //    return;
                //}
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
        }


    }
}
