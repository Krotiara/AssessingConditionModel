using OfficeOpenXml;
using WebMVC.Models;
using Interfaces;

namespace WebMVC.Services
{
    public class AgingDynamicsSaveService : IAgingDynamicsSaveService
    {
        private readonly List<string> columNames;

        public AgingDynamicsSaveService()
        {
            columNames = new List<string> { "Пациент", "Тип воздействия", "Наименование препарата",
                "Начало воздействия", "Окончание воздействия",
                "Возраст до","Возраст после", "Биовозраст до",
                "Биовозраст после", "Дельта до", "Дельта после", 
                "Дельта дельты", "Состояние до", "Состояние после" };
        }

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
                        ExcelWorksheet worksheet =  
                            excel.Workbook.Worksheets.Add( $"{count++} {group.Key.MedicineName}");
                        worksheet.InsertRow(1, group.Count() + 1);
                        worksheet.InsertColumn(1, columNames.Count()+1);
                        for (int j = 0; j < columNames.Count; j++)
                            worksheet.Cells[1,j+1].Value = columNames[j];
                        int rowsIndex = 2;
                        foreach(AgingDynamics aD in group)
                        {
                            List<string> row = GetExportString(group.Key.InfluenceType, group.Key.MedicineName, aD);
                            for(int j = 0; j < columNames.Count();j++)
                                worksheet.Cells[rowsIndex,j+1].Value = row[j];
                            rowsIndex++;
                        }
                    }
#warning TODO установка пути
                    FileInfo excelFile = new FileInfo("C:\\test.xlsx");
                    excel.SaveAs(excelFile);
                }

                return null;
            }
            catch(Exception ex)
            {
                throw new NotImplementedException(); //TODO
            }
        }


        private List<string> GetExportString(InfluenceTypes iT, string medicineName, AgingDynamics aD)
        {
            return new List<string>()
            {
                aD.PatientId.ToString(), iT.GetDisplayAttributeValue(), medicineName,
                aD.StartTimestamp.ToShortDateString(), aD.EndTimestamp.ToShortDateString(),
                aD.AgentStateInInfluenceStart.Age.ToString(), aD.AgentStateInInfluenceEnd.Age.ToString(),
                aD.AgentStateInInfluenceStart.BioAge.ToString(), aD.AgentStateInInfluenceEnd.BioAge.ToString(),
                aD.StartDelta.ToString(), aD.EndDelta.ToString(), (aD.EndDelta - aD.StartDelta).ToString(),
                aD.AgentStateInInfluenceStart.BioAgeState.GetDisplayAttributeValue(),
                aD.AgentStateInInfluenceEnd.BioAgeState.GetDisplayAttributeValue()
            };
        }
    }
}
