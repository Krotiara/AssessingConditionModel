using Interfaces;
using OfficeOpenXml;

namespace PatientDataHandler.API.Models
{
    public class ExcelDataProvider : IDataProvider
    {
        private List<int> headersColumnsIndexes = new List<int>() {1, 2};

        public ExcelDataProvider()
        {
           
        }

        public IPatientData ParseData(string filePath)
        {
            (List<List<string>>, Dictionary<string, int>) datas = GetExcelData(filePath, headersColumnsIndexes);
            List<List<string>> data = datas.Item1;
            Dictionary<string, int> headersColumnIndexes = datas.Item2;
            DataPreprocessor dataPreprocessor = new DataPreprocessor();
            List<List<string>> processedData = dataPreprocessor.PreProcessData(data, ref headersColumnIndexes);
            
            //TODO выуживание всей информации. Продумать map.
            throw new NotImplementedException();
        }


        private IEnumerable<IPatientParameter> ParseExcelData(string filePath)
        {
            Dictionary<int, IPatientParameter> patientParameters = new Dictionary<int, IPatientParameter>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(filePath)))
            {
                //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
                ExcelWorksheet myWorksheet = xlPackage.Workbook.Worksheets.First(); //Если брать по индексу 0, то он пропускает некоторые пустые столбцы, из-за чего слетают индексы.
                int totalRows = myWorksheet.Dimension.End.Row;
                int totalColumns = myWorksheet.Dimension.End.Column;

#warning Возможно здесь индексирование с 1, нужна проверка.
                List<string> headers = myWorksheet
                        .Cells[0, 1, 0, totalColumns] 
                        .Select(c => c.Value == null ? string.Empty : c.Value.ToString().Trim().ToLower())
                        .ToList();

                bool isDynamicRows = false;

                for (int rowNum = 1; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    List<string> row = myWorksheet
                        .Cells[rowNum, 1, rowNum, totalColumns]
                        .Select(c => c.Value == null ? string.Empty : c.Value.ToString().Trim())
                        .ToList();

                    if (row[0] == "динамика")
                    {
                        isDynamicRows = true;
                        continue;
                    }
                    //TODO сейчас неправльно. Будто одна строка - один параметр. Сделать на создание большого набора, а затем group by patient id
                    IPatientParameter parameter = null;
                    int id = int.Parse(row[0]);
                    if (isDynamicRows)
                        parameter = patientParameters[id];
                    else
                    {
                        parameter = new PatientParameter() { Timestamp = DateTime.Now }; //TODO Now заменить.
                        parameter.PatientId = int.Parse(row[0]);
                        parameter.PositiveDynamicCoef = 1; //TODO нужно указывать во входных данных.
                    }

                    for (int j = 1; j < row.Count; j++)
                    {
                        if(isDynamicRows)
                            parameter.DynamicValue = row[j];
                        else
                            parameter.Value = row[j];
                    }
                }
                return (data, headersColumnIndexes);
            }
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
