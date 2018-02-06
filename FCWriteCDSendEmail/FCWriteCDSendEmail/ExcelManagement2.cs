using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCWriteCDSendEmail
{
    class ExcelManagement2
    {

        public ExcelManagement2()
        {
            ;
        }

        public void CreateExcelFile(Dictionary<string, DataTable> dataSource, string fileName)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "esynergy";
            hssfworkbook.DocumentSummaryInformation = dsi;

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "FC_WRITE_CD_STORE_MAP";
            hssfworkbook.SummaryInformation = si;

            foreach (KeyValuePair<string, DataTable> each in dataSource)
            {
                ISheet sheetExcel = hssfworkbook.CreateSheet(each.Key);

                IRow firstRowExcel = sheetExcel.CreateRow(0);
                ICell firstCellExcel = firstRowExcel.CreateCell(0);
                firstCellExcel.SetCellValue("Store Code");
                firstCellExcel.CellStyle.Alignment = HorizontalAlignment.Center;
                ICell secondCellExcel = firstRowExcel.CreateCell(1);
                secondCellExcel.SetCellValue("Store Name");
                secondCellExcel.CellStyle.Alignment = HorizontalAlignment.Center;

                int rowNum = 1;
                foreach (DataRow row in each.Value.Rows)
                {
                    IRow excelRow = sheetExcel.CreateRow(rowNum);
                    ICell excelCell1 = excelRow.CreateCell(0);
                    excelCell1.SetCellValue(row[0].ToString());
                    excelCell1.CellStyle.Alignment = HorizontalAlignment.Center;
                    ICell excelCell2 = excelRow.CreateCell(1);
                    excelCell2.SetCellValue(row[1].ToString());
                    excelCell2.CellStyle.Alignment = HorizontalAlignment.Center;      
                    rowNum++;
                }

                sheetExcel.AutoSizeColumn(0);
                sheetExcel.AutoSizeColumn(1);
                sheetExcel.SetColumnWidth(1, sheetExcel.GetColumnWidth(1) + 1000);
            }

            FileStream file = new FileStream(fileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }

    }
}
