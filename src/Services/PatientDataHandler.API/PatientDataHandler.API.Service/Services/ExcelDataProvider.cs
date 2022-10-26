using ExcelDataReader;
using Interfaces;
using OfficeOpenXml;
using PatientDataHandler.API.Entities;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace PatientDataHandler.API.Service.Services
{
    /// <summary>
    /// Парсер тестового формата данных.
    /// </summary>
    public class ExcelDataProvider : IDataProvider
    {

       
        public ExcelDataProvider()
        {

        }
 

        public IList<Influence> ParseData(string filePath)
        {
            //TODO add try catch
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ParseData(ms.ToArray());
            }
        }


        public IList<Influence> ParseData(byte[] bytesData)
        {
            try
            {
                IList<IList<string>> rawData = LoadData(bytesData);
                DataPreprocessor dataPreprocessor = new DataPreprocessor();
                rawData = dataPreprocessor.PreProcessData(rawData);
                IList<Influence> data = ParseExcelData(rawData[0], rawData.Skip(1).ToList());
                return data;
            }
            catch(Exception ex)
            {
                throw new ParseInfluenceDataException("Parse data exception", ex);
            }
        }


        private IList<Influence> ParseExcelData(IList<string> headers, IList<IList<string>> data)
        {
            Dictionary<int, Influence> patientsInfluences  = new Dictionary<int, Influence>();
            bool isDynamicRows = false;

#warning Узкое место в названии препарата.
            int groupIndex = headers.IndexOf("группа");

            IList<ParameterNames> headerParamsNames = headers
                .Select(x => x.GetParameterByDescription())
                .ToList();

            int parameterTimestampIndex = headerParamsNames.IndexOf(ParameterNames.ParameterTimestamp);

            for (int rowNum = 0; rowNum <= data.Count; rowNum++) //select starting row here
            {
                try
                {
                    IList<string> row = data[rowNum];

                    DateTime parameterTimestamp = parameterTimestampIndex == -1 || row[parameterTimestampIndex] =="" ? 
                        DateTime.MinValue : DateTime.Parse(row[parameterTimestampIndex]);
                    string influenceName = data[rowNum][groupIndex];
                    
                    if (row[0] == "динамика")
                    {
                        isDynamicRows = true;
                        continue;
                    }

                    Influence influenceData = null;
                    int id = int.Parse(row[0]);
                    if (isDynamicRows)
                        influenceData = patientsInfluences[id];
                    else
                    {
                        influenceData = new Influence()
                        {
                            InfluenceType = InfluenceTypes.BiologicallyActiveAdditive,
                            MedicineName = influenceName,
                            StartTimestamp = DateTime.MinValue, //TODO указывать во входных данных,
                            EndTimestamp = DateTime.MinValue, //TODO указывать во входных данных
                            PatientId = id,
                            Patient = new Patient()
                            {
                                MedicalHistoryNumber = id,
                                Name = "",
                                Birthday = DateTime.MinValue
                            }
                        };
                        patientsInfluences[id] = influenceData;
                    }
                     
                    Parallel.For(1, row.Count, j =>
                    {
                        try
                        {
                            if (headerParamsNames[j] == ParameterNames.None)
                                return;
                            ParameterNames parameterName = headerParamsNames[j]; //Доступ к общему листу problem

                            ConcurrentDictionary<ParameterNames, PatientParameter> curDict = isDynamicRows ? 
                            influenceData.DynamicParameters : influenceData.StartParameters;

                            if (!curDict.ContainsKey(parameterName) && row[j] != "") //Не добавлять пустые значения параметров
                            {
                                curDict[parameterName] = new PatientParameter(parameterName)
                                {
                                    Timestamp = parameterTimestamp,
                                    PatientId = id,
                                    PositiveDynamicCoef = 1, //TODO нужно указывать во входных данных.
                                    IsDynamic = isDynamicRows,
                                    Value = row[j]
                                };
                            };
                        }
                        catch (KeyNotFoundException ex)
                        {
                            //Не найден Parameters
                            //TODO log
                            return;
                        }
                        catch (Exception ex)
                        {
                            //TODO add log
                            return;
                        }
                    });

                    influenceData.Patient.Gender = influenceData.StartParameters.ContainsKey(ParameterNames.Gender) ? 
                        GetPatientGender(influenceData.StartParameters[ParameterNames.Gender]) : GenderEnum.None;
                    
                }
                catch(Exception ex)
                {
                    //TODO add log
                    continue;
                }
            }

            foreach(KeyValuePair<int, Influence> inf in patientsInfluences)
                SetInfluenceTimeByParamsTime(inf.Value);

            return patientsInfluences
                .Values
                .ToList();
        }


#warning Временное решение для указания даты воздействия.
        private void SetInfluenceTimeByParamsTime(Influence influence)
        {
            DateTime start = influence.StartParameters.Values.OrderBy(x => x.Timestamp).First().Timestamp;
            DateTime end = influence.DynamicParameters.Count > 0 ? 
                influence.DynamicParameters.Values.OrderBy(x => x.Timestamp).Last().Timestamp : DateTime.MaxValue;
            influence.StartTimestamp = start;
            influence.EndTimestamp = end;
        }

#warning Временное решение.
        private GenderEnum GetPatientGender(PatientParameter genderParameter)
        {
            if (genderParameter.ParameterName != ParameterNames.Gender)
                throw new NotImplementedException(); //TODO
            string val = genderParameter.Value;
            if (val == "ж" || val.Contains("жен"))
                return GenderEnum.Female;
            else if (val == "м" || val.Contains("муж"))
                return GenderEnum.Male;
            else return GenderEnum.None;
        }


        private IList<IList<string>> LoadData(byte[] bytesData)
        {
            //TODO try catch
            IList<IList<string>> data = new List<IList<string>>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (MemoryStream stream = new MemoryStream(bytesData))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each ROW
                    {
                        IList<string> row = new List<string>();
                        for (int column = 0; column < reader.FieldCount; column++)
                        {
                            object value = reader.GetValue(column);
                            row.Add(value == null ? "" : value.ToString());
                        }
                        data.Add(row);
                    }
                }
            }
            return data;
        }

       
    }
}
