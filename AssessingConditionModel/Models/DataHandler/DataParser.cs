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
        /// <summary>
        /// Словарь соответствий колонок входных данных и относительных путей до соответсвующих свойств объекта Patient. 
        /// </summary>
        private readonly Dictionary<string, string> matchingPropertiesNames = new Dictionary<string, string>()
        {
            {"номер истории болезни", "MedicalHistoryNumber" },
            {"Кашель","ClinicalParameters.IsCough" },
            // TODO соответствия и затем дебаг.
        };


        public void TODO()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(@"C:\Users\Krotiara\Downloads\Telegram Desktop\статкарта.xlsx")))
{
                //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
                ExcelWorksheet myWorksheet = xlPackage.Workbook.Worksheets[1]; //Если брать по индексу 0, то он пропускает некоторые пустые столбцы, из-за чего слетают индексы.
                int totalRows = myWorksheet.Dimension.End.Row;
                int totalColumns = myWorksheet.Dimension.End.Column;

                Dictionary<string, int> headersColumnIndexes = ExcelParsePatientsHeaders(myWorksheet, new List<int>() { 1, 2 }, totalColumns);
                List<Patient> patients = new List<Patient>();
                for (int rowNum = 3; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    List<string> row = myWorksheet
                        .Cells[rowNum, 1, rowNum, totalColumns]
                        .Select(c => c.Value == null ? string.Empty : c.Value.ToString())
                        .ToList();

                    try
                    {
                        Patient p = ProcessPatientRow(row, headersColumnIndexes);
                        patients.Add(p);
                    }
                    catch(System.FormatException e)
                    {
                        continue; //TODO - Разобраться, почему возникла на какой-то строке длиной 16. с ЦРБ, который не находится в документе.
                    }
                }
            }
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
            foreach(string header in matchingPropertiesNames.Keys)
            {
                int columnIndex = headersColumnIndexes[header];
                string propertyName = matchingPropertiesNames[header];
                string value = row[columnIndex];     
                (PropertyInfo, object) propertyInfoData = GetPropertyInfo(patient, propertyName);
                object convertedValue = ConvertValue(value, propertyInfoData.Item1.PropertyType);
                propertyInfoData.Item1.SetValue(propertyInfoData.Item2, convertedValue);
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
                return GetPropertyInfo(nestedPropertyValue, pathPropertiesNames[1]);
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
                    .Select(c => c.Value == null ? string.Empty : c.Value.ToString()/*.ToLower()*/)
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
