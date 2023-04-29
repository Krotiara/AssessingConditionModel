﻿using ExcelDataReader;
using Interfaces;
using Interfaces.Service;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using PatientDataHandler.API.Entities;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace PatientDataHandler.API.Service.Services
{
    //TODO переделать под универсальный формат с проверкой наличия параметров из бд.

    /// <summary>
    /// Парсер тестового формата данных.
    /// </summary>
    public class ExcelDataProvider : IDataProvider
    {
        private readonly ParseDataSettings _settings; //Временное решение.

        public ExcelDataProvider(IOptions<ParseDataSettings> settings)
        {
            _settings = settings.Value;
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
            int groupIndex = headers.IndexOf(_settings.Group);

            //TODO вынести в settings и в парсинг данных добавить сопоставление описания и имени параметра.
            int parameterTimestampIndex = headers.IndexOf(_settings.Timestamp);

            for (int rowNum = 0; rowNum < data.Count; rowNum++) //select starting row here
            {
                try
                {
                    IList<string> row = data[rowNum];

                    DateTime parameterTimestamp = parameterTimestampIndex == -1 || row[parameterTimestampIndex] =="" ? 
                        DateTime.MinValue : DateTime.Parse(row[parameterTimestampIndex]);
                    string influenceName = groupIndex == -1 ? "" : data[rowNum][groupIndex];
                    
                    if (row[0] == _settings.Dynamic)
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
#warning Костыль с InfluenceType
                            InfluenceType = InfluenceTypes.BiologicallyActiveAdditive,
                            MedicineName = influenceName,
                            StartTimestamp = DateTime.MinValue, //TODO указывать во входных данных,
                            EndTimestamp = DateTime.MinValue, //TODO указывать во входных данных
                            PatientId = id,
                            Patient = new Patient()
                            {
                                Id = id,
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
                            //if (headerParamsNames[j] == ParameterNames.None) В новой реализации будет заноситься и Id.
                            //    return;
                            string parameterName = headers[j]; //Доступ к общему листу problem

                            ConcurrentDictionary<string, PatientParameter> curDict = isDynamicRows ? 
                            influenceData.DynamicParameters : influenceData.StartParameters;

                            if (!curDict.ContainsKey(parameterName) && row[j] != "") //Не добавлять пустые значения параметров
                            {
                                curDict[parameterName] = new PatientParameter(parameterName)
                                {
                                    Timestamp = parameterTimestamp,
                                    PatientId = id,
                                    IsDynamic = isDynamicRows,
                                    Value = row[j]
                                };
                            };
                        }
                        catch (KeyNotFoundException ex)
                        {
                            //TODO log
                            throw new ParseInfluenceDataException($"Error with getting header for column with number = {j}", ex);
                        }
                        catch (Exception ex)
                        {
                            //TODO add log
                            throw new ParseInfluenceDataException($"Unexpected error for column with number = {j}", ex);
                        }
                    });

                    influenceData.Patient.Gender = influenceData.StartParameters.ContainsKey(_settings.Gender) ? 
                        GetPatientGender(influenceData.StartParameters[_settings.Gender]) : GenderEnum.None;
                    
                }
                catch(Exception ex)
                {
                    //TODO add log
                    throw new ParseInfluenceDataException($"Error for rowNum = {rowNum}", ex);
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
            if (genderParameter.Name != _settings.Gender)
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
