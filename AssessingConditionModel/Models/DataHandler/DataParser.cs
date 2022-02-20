using AssessingConditionModel.Models.PatientModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.DataHandler
{
    public class DataParser
    {

        private MatchingProperties mp;

        public DataParser()
        {
            mp = new MatchingProperties();
        }

        public (List<List<string>>, Dictionary<string, int>) GetExcelData(string path, List<int> headersColumnsIndexes)
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


        public List<Patient> LoadData()
        {
            (List<List<string>>, Dictionary<string, int>) datas = GetExcelData(@"C:\Users\Krotiara\Desktop\Аспирантура\статкартаЛист2.xlsx", new List<int>() { 1, 2 });
            List<List<string>> data = datas.Item1;
            Dictionary<string, int> headersColumnIndexes = datas.Item2;

            List<Patient> patients = new List<Patient>();

            DataPreprocessor dataPreprocessor = new DataPreprocessor();
            List<List<string>> processedData = dataPreprocessor.PreProcessData(data, ref headersColumnIndexes);

            foreach (List<string> row in processedData) //select starting row here
            {
                //try
                //{
                    Patient p = ProcessPatientRow(row, headersColumnIndexes);
                    patients.Add(p);
                //}
                //catch (Exception e)
                //{
                //    if (e is FormatException || e is ArgumentOutOfRangeException)
                //        continue;//Выбрасывается при некорректной строке данных пациента. (или при строке, не относящихся к данным пациента).
                //    else throw;
                //}
            }
            return patients;
        }
    

        

        /// <summary>
        /// Метод обрабатывает отслеживаемые параметры и присваивает им значения на основе переданной строки данных.
        /// </summary>
        /// <param name="row">Строка данных.</param>
        /// <param name="headersColumnIndexes">Словарь соответсвия навзаний заголовков и индексов соответсвующим им колонок.</param>
        /// <returns></returns>
        private Patient ProcessPatientRow(List<string> row, Dictionary<string, int> headersColumnIndexes)
        {
            Patient patient = new Patient();

            Dictionary<string, string> matchingProperties = mp.MatchingPropertiesNames
                .Where(x => headersColumnIndexes.Keys.Contains(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (string header in matchingProperties.Keys)
            {
                int columnIndex = headersColumnIndexes[header];
                string propertyName = mp.MatchingPropertiesNames[header];
                string value = row[columnIndex];
                (PropertyInfo, object) propertyInfoData = GetPropertyInfo(patient, propertyName);
                try
                {         
                    object convertedValue = ConvertValue(value, propertyInfoData.Item1.PropertyType);
                    propertyInfoData.Item1.SetValue(propertyInfoData.Item2, convertedValue);
                }
                catch(System.FormatException e)
                {
                    throw new SetPropertyValueException(
                        $"Cant set property {propertyName} with type {propertyInfoData.Item1.PropertyType.Name}." +
                        $"Value to set = {value}\n" +
                        $"Header = {header}\n" +
                        $"ColumnIndex={columnIndex}");
                }
               
            }
            return patient;
        }


        private object ConvertValue(string value, Type propertyType)
        {
            if(propertyType == typeof(bool))
            {
                if (value.Equals("1"))
                    value = "True";
                else if (value.Equals("0"))
                    value = "False";
            }
            return Convert.ChangeType(value, propertyType);
        }


        /// <summary>
        /// На основе относительного пути до свойства объекта вычисляется  PropertyInfo этого свойства и relative объект этого свойства.
        /// </summary>
        /// <param name="obj">Root объект.</param>
        /// <param name="propertyPath">Относительный путь до свойства, разделитель - точка.</param>
        /// <returns>PropertyInfo свойства и relative объект этого свойства (с учетом вложенности Generic типов.)</returns>
        private (PropertyInfo, object) GetPropertyInfo(object obj, string propertyPath)
        {
            if(obj == null) throw new ArgumentException("Value cannot be null.", "patient");
            if (propertyPath == null) throw new ArgumentException("Value cannot be null.", "propertyPath");
            if(propertyPath.Contains('.'))
            {
                List<string> pathPropertiesNames = propertyPath.Split('.').ToList();
                PropertyInfo currentPInfo = obj.GetType().GetProperty(pathPropertiesNames[0]);
                object nestedPropertyValue = currentPInfo.GetValue(obj, null);
                return GetPropertyInfo(nestedPropertyValue, propertyPath.Replace($"{pathPropertiesNames[0]}.",""));
            }
            else
            {
                List<string> pathPropertiesNames = propertyPath.Split('.').ToList();
                PropertyInfo currentPInfo = obj.GetType().GetProperty(pathPropertiesNames.First());
                return (currentPInfo, obj);
            }       
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns>Словарь, ключ - имя столбца, значение - индекс столбца</returns>
        public Dictionary<string, int> ExcelParsePatientsHeaders(ExcelWorksheet myWorksheet, List<int> headersRowsIndexes, int totalColumnsNumber)
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
            foreach(int headerRowIndex in headersRowsIndexes)
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
