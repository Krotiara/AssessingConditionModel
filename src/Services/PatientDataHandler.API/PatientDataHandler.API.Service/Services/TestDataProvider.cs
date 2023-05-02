using ExcelDataReader;
using Interfaces;
using Interfaces.Requests;
using Interfaces.Service;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using PatientDataHandler.API.Entities;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PatientDataHandler.API.Service.Services
{
    //TODO переделать под универсальный формат с проверкой наличия параметров из бд.

    /// <summary>
    /// Парсер тестового формата данных.
    /// </summary>
    public class TestDataProvider : IDataProvider
    {
        private readonly ParseDataSettings _settings; //Временное решение.

        public TestDataProvider(IOptions<ParseDataSettings> settings)
        {
            _settings = settings.Value;
        }
 
   
        public IList<Influence> ParseData(IAddInfluencesRequest addDataRequest)
        {
            try
            {
                string data = Encoding.UTF8.GetString(addDataRequest.Content);
                var rows = data.Split("\r\n");
                IList<string[]> rawData = new List<string[]> { };
                foreach (string row in rows)
                    rawData.Add(row.Split(";"));

                DataPreprocessor dataPreprocessor = new DataPreprocessor();
                rawData = dataPreprocessor.PreProcessData(rawData);
                IList<Influence> res = ParseData(addDataRequest, rawData[0], rawData.Skip(1).ToList());
                return res;
            }
            catch(Exception ex)
            {
                throw new ParseInfluenceDataException("Parse data exception", ex);
            }
        }


        private IList<Influence> ParseData(IAddInfluencesRequest addDataRequest, IList<string> headers, IList<string[]> data)
        {
            Dictionary<int, Influence> patientsInfluences  = new Dictionary<int, Influence>();
            bool isDynamicRows = false;

            //TODO вынести в settings и в парсинг данных добавить сопоставление описания и имени параметра.
            int parameterTimestampIndex = headers.IndexOf(_settings.Timestamp);

            for (int rowNum = 0; rowNum < data.Count; rowNum++) //select starting row here
            {
                try
                {
                    IList<string> row = data[rowNum];

                    DateTime parameterTimestamp = parameterTimestampIndex == -1 || row[parameterTimestampIndex] =="" ?
                        addDataRequest.StartTimestamp : DateTime.Parse(row[parameterTimestampIndex]);

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
#warning убрать Birthday = DateTime.MinValue
                            PatientId = id,
                            Patient = new Patient()
                            {
                                Id = id,
                                MedicalOrganization = addDataRequest.Affiliation
                            },
                            InfluenceType = addDataRequest.InfluenceType,
                            MedicineName = addDataRequest.MedicineName,
                            MedicalOrganization = addDataRequest.Affiliation,
                            StartTimestamp = addDataRequest.StartTimestamp,
                            EndTimestamp = (DateTime)addDataRequest.EndTimestamp //TODO убрать явное приведение, везде заменить на Nullable
                        };
                        patientsInfluences[id] = influenceData;
                    }
                     
                    Parallel.For(1, row.Count, j =>
                    {
                        try
                        {
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
                                    Value = row[j],
                                    PatientAffiliation = addDataRequest.Affiliation,
                                    Name = parameterName
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

            return patientsInfluences
                .Values
                .ToList();
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
    }
}
