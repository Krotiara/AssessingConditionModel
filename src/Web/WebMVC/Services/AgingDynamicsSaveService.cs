using OfficeOpenXml;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class AgingDynamicsSaveService : IAgingDynamicsSaveService
    {

        public string SaveToExcelFile(CommonAgingDynamics dynamics)
        {
#warning Тестовая версия.
            try
            {
                var groupedDynamics = dynamics.AgingDynamics.
                    GroupBy(x => new { x.InfluenceType, x.MedicineName, x.StartTimestamp, x.EndTimestamp });
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excel = new ExcelPackage())
                {
                    int count = 1;
                    foreach(var group in groupedDynamics)
                    {
                        excel.Workbook.Worksheets.Add( $"{count++} {group.Key.MedicineName}");
                    }
#warning TODO установка пути
                    FileInfo excelFile = new FileInfo(@"C:\test.xlsx");
                    excel.SaveAs(excelFile);
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new NotImplementedException(); //TODO
            }
        }
    }
}
