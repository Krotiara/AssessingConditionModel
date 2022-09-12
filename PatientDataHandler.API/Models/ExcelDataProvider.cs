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

        //Пока что версия из монолита.
        private (List<List<string>>, Dictionary<string, int>) GetExcelData(string path, List<int> headersColumnsIndexes)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(path)))
            {
                //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
                ExcelWorksheet myWorksheet = xlPackage.Workbook.Worksheets.First(); //Если брать по индексу 0, то он пропускает некоторые пустые столбцы, из-за чего слетают индексы.
                int totalRows = myWorksheet.Dimension.End.Row;
                int totalColumns = myWorksheet.Dimension.End.Column;

                Dictionary<string, int> headersColumnIndexes = ExcelParsePatientsHeaders(myWorksheet, headersColumnsIndexes, totalColumns);

                List<List<string>> data = new List<List<string>>();
                int startDataIndex = headersColumnsIndexes.Max() + 1;
                for (int rowNum = startDataIndex; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    List<string> row = myWorksheet
                        .Cells[rowNum, 1, rowNum, totalColumns]
                        .Select(c => c.Value == null ? string.Empty : c.Value.ToString().Trim())
                        .ToList();
                    data.Add(row);
                }
                return (data, headersColumnIndexes);
            }
        }


        //Пока что версия из монолита.
        /// <summary>
        ///
        /// </summary>
        /// <returns>Словарь, ключ - имя столбца, значение - индекс столбца</returns>
        private Dictionary<string, int> ExcelParsePatientsHeaders(ExcelWorksheet myWorksheet, List<int> headersRowsIndexes, int totalColumnsNumber)
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach (int headerRowIndex in headersRowsIndexes)
            {
                List<string> row = myWorksheet
                    .Cells[headerRowIndex, 1, headerRowIndex, totalColumnsNumber]
                    .Select(c => c.Value == null ? string.Empty : c.Value.ToString().Trim().ToLower())
                    .ToList();
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Equals(""))
                        continue;
                    else res[row[i]] = i;
                }

            }
            return res;
        }
    }
}
