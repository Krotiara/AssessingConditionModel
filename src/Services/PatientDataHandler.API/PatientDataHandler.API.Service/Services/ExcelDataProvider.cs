using ExcelDataReader;
using Interfaces;
using OfficeOpenXml;
using PatientDataHandler.API.Entities;

namespace PatientDataHandler.API.Service.Services
{
    /// <summary>
    /// Парсер тестового формата данных.
    /// </summary>
    public class ExcelDataProvider : IDataProvider
    {

        private readonly IPropertiesMatcherService propertiesMatcherService;

        public ExcelDataProvider(IPropertiesMatcherService propertiesMatcherService)
        {
           this.propertiesMatcherService = propertiesMatcherService;
        }
 

        public IList<IPatientData> ParseData(string filePath)
        {
            //TODO add try catch
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ParseData(ms.ToArray());
            }
        }


        public IList<IPatientData> ParseData(byte[] bytesData)
        {
            IList<IList<string>> rawData = LoadData(bytesData);
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

                    string influenceName = data[rowNum][headers.IndexOf("группа")];
                    Influence influence = new Influence()
                    {
                        InfluenceType = InfluenceTypes.BiologicallyActiveAdditive,
                        MedicineName = influenceName,
                        StartTimestamp = DateTime.MinValue, //TODO указывать во входных данных,
                        EndTimestamp = DateTime.MinValue //TODO указывать во входных данных
                    };

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
                            Parameters = new List<IPatientParameter>(),
                            Timestamp = DateTime.MinValue, //TODO указывать во входных данных
                            Influence = influence                 
                        };
                        patientData.Influence.PatientId = id;
                        patientParameters[id] = patientData;
                    }

                    for (int j = 1; j < row.Count; j++)
                    {
                        try
                        {
                            Parameters parameterName = propertiesMatcherService.GetParameterBy(headers[j]);
                            IPatientParameter patientParameter = patientData.Parameters.FirstOrDefault(x => x.ParameterName == parameterName);
                            if (patientParameter == null)
                            {
                                patientParameter = new PatientParameter(parameterName)
                                {
                                    Timestamp = DateTime.MinValue, //TODO  нужно указывать во входных данных.
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
                        catch(KeyNotFoundException ex)
                        {
                            //Не найден Parameters
                            //TODO log
                            continue;
                        }
                        catch (Exception ex)
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
