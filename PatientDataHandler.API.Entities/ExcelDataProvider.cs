using ExcelDataReader;
using Interfaces;

namespace PatientDataHandler.API.Entities
{
    public class ExcelDataProvider : IDataProvider
    {
        public ExcelDataProvider()
        {
           
        }
 
        public IList<IPatientData> ParseData(string filePath)
        {
            //TODO add try catch
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            return ParseData(stream);
        }

        public IList<IPatientData> ParseData(Stream stream)
        {
            IList<IList<string>> rawData = LoadData(stream);
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


        private IList<IList<string>> LoadData(Stream stream)
        {
            //TODO try catch
            IList<IList<string>> data = new List<IList<string>>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using(stream)
            {
#warning Здесь ошибка с MemoryStream System.IO.InvalidDataException: Offset to Central Directory cannot be held in an Int64.
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
