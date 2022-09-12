using Interfaces;
using OfficeOpenXml;

namespace PatientDataHandler.API.Models
{
    public class ExcelDataProvider : IDataProvider
    {
        public ExcelDataProvider()
        {
           
        }
 
        public IList<IPatientData> ParseData(string filePath)
        {
            IList<IList<string>> rawData = LoadData(filePath);
            DataPreprocessor dataPreprocessor = new DataPreprocessor();
            rawData = dataPreprocessor.PreProcessData(rawData);
            IList<IPatientData> data = ParseExcelData(rawData[0], rawData.Skip(1).ToList());
            return data;
        }

        private IList<IPatientData> ParseExcelData(IList<string> headers, IList<IList<string>> data)
        {
            Dictionary<int, IPatientData> patientParameters = new Dictionary<int, IPatientData>();
            bool isDynamicRows = false;
            for (int rowNum = 0; rowNum <= data.Count; rowNum++) //select starting row here
            {
                try
                {
                    IList<string> row = data[rowNum];

                    if (row[0] == "динамика")
                    {
                        isDynamicRows = true;
                        continue;
                    }

                    IPatientData patientData = null;
                    int id = int.Parse(row[0]);
                    if (isDynamicRows)
                        patientData = patientParameters[id];
                    else
                    {
                        patientData = new PatientData()
                        {
                            PatientId = id,
                            Parameters = new List<IPatientParameter>()
                        };
                        patientParameters[id] = patientData;
                    }

                    for (int j = 1; j < row.Count; j++)
                    {
                        try
                        {
                            IPatientParameter patientParameter = patientData.Parameters.FirstOrDefault(x => x.Name == headers[j]);
                            if (patientParameter == null)
                            {
                                patientParameter = new PatientParameter()
                                {
                                    Name = headers[j],
                                    Timestamp = DateTime.Now, //TODO  нужно указывать во входных данных.
                                    PatientId = id,
                                    PositiveDynamicCoef = 1 //TODO нужно указывать во входных данных.
                                };
                                patientData.Parameters.Add(patientParameter);
                            };

                            if (isDynamicRows)
                                patientParameter.DynamicValue = row[j];
                            else
                                patientParameter.Value = row[j];
                        }
                        catch(Exception ex)
                        {
                            //TODO add log
                            continue;
                        }
                    }
                }
                catch(Exception ex)
                {
                    //TODO add log
                    continue;
                }
            }
               
            return patientParameters.Values.ToList();
        }


        private IList<IList<string>> LoadData(string filePath)
        {
            IList<IList<string>> data = new List<IList<string>>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(filePath)))
            {
                //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
                ExcelWorksheet myWorksheet = xlPackage.Workbook.Worksheets.First(); //Если брать по индексу 0, то он пропускает некоторые пустые столбцы, из-за чего слетают индексы.
                int totalRows = myWorksheet.Dimension.End.Row;
                int totalColumns = myWorksheet.Dimension.End.Column;
                for (int rowNum = 0; rowNum <= totalRows; rowNum++) //select starting row here
                    data.Add(myWorksheet
                        .Cells[rowNum, 1, rowNum, totalColumns]
                        .Select(c => c.Value == null ? string.Empty : c.Value.ToString().Trim())
                        .ToList());
            }
            return data;
        }

      

        ////Пока что версия из монолита.
        //private (List<List<string>>, Dictionary<string, int>) GetExcelData(string path, List<int> headersColumnsIndexes)
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(path)))
        //    {
        //        //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
        //        ExcelWorksheet myWorksheet = xlPackage.Workbook.Worksheets.First(); //Если брать по индексу 0, то он пропускает некоторые пустые столбцы, из-за чего слетают индексы.
        //        int totalRows = myWorksheet.Dimension.End.Row;
        //        int totalColumns = myWorksheet.Dimension.End.Column;

        //        Dictionary<string, int> headersColumnIndexes = ExcelParsePatientsHeaders(myWorksheet, headersColumnsIndexes, totalColumns);

        //        List<List<string>> data = new List<List<string>>();
        //        int startDataIndex = headersColumnsIndexes.Max() + 1;
        //        for (int rowNum = startDataIndex; rowNum <= totalRows; rowNum++) //select starting row here
        //        {
        //            List<string> row = myWorksheet
        //                .Cells[rowNum, 1, rowNum, totalColumns]
        //                .Select(c => c.Value == null ? string.Empty : c.Value.ToString().Trim())
        //                .ToList();
        //            data.Add(row);
        //        }
        //        return (data, headersColumnIndexes);
        //    }
        //}


        ////Пока что версия из монолита.
        ///// <summary>
        /////
        ///// </summary>
        ///// <returns>Словарь, ключ - имя столбца, значение - индекс столбца</returns>
        //private Dictionary<string, int> ExcelParsePatientsHeaders(ExcelWorksheet myWorksheet, int totalColumnsNumber)
        //{
        //    Dictionary<string, int> res = new Dictionary<string, int>();

        //        List<string> row = myWorksheet
        //            .Cells[headerRowIndex, 1, headerRowIndex, totalColumnsNumber]
        //            .Select(c => c.Value == null ? string.Empty : c.Value.ToString().Trim().ToLower())
        //            .ToList();
        //        for (int i = 0; i < row.Count; i++)
        //        {
        //            if (row[i].Equals(""))
        //                continue;
        //            else res[row[i]] = i;
        //        }

        //    return res;
        //}
    }
}
