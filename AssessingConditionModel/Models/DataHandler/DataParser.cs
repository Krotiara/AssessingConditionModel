using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.DataHandler
{
    public class DataParser
    {

        public void TODO()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(@"C:\Users\Krotiara\Downloads\Telegram Desktop\статкарта.xlsx")))
{
                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                var sb = new StringBuilder(); //this is your data
                for (int rowNum = 1; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                    sb.AppendLine(string.Join(",", row));
                }
            }
            
            
        }
    }
}
