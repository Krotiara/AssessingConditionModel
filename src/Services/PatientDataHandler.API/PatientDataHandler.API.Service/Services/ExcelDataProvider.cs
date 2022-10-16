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
 

        public IList<IPatientData<IPatientParameter, IPatient, IInfluence>> ParseData(string filePath)
        {
            //TODO add try catch
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ParseData(ms.ToArray());
            }
        }


        public IList<IPatientData<IPatientParameter, IPatient, IInfluence>> ParseData(byte[] bytesData)
        {
            IList<IList<string>> rawData = LoadData(bytesData);
            DataPreprocessor dataPreprocessor = new DataPreprocessor();
            rawData = dataPreprocessor.PreProcessData(rawData);
            IList<IPatientData<IPatientParameter, IPatient, IInfluence>> data = ParseExcelData(rawData[0], rawData.Skip(1).ToList());
            return data;
        }


        private IList<IPatientData<IPatientParameter, IPatient, IInfluence>> ParseExcelData(IList<string> headers, IList<IList<string>> data)
        {
            Dictionary<int, IPatientData<IPatientParameter, IPatient, IInfluence>> patientParameters = 
                new Dictionary<int, IPatientData<IPatientParameter, IPatient, IInfluence>>();
            bool isDynamicRows = false;
#warning Узкое место в названии препарата.
            int groupIndex = headers.IndexOf("группа");

            IList<ParameterNames> headerParamsNames = headers
                .Select(x => x.GetParameterByDescription())
                .ToList();

            for (int rowNum = 0; rowNum <= data.Count; rowNum++) //select starting row here
            {
                try
                {
                    IList<string> row = data[rowNum];
                    string influenceName = data[rowNum][groupIndex];
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

                    IPatientData<IPatientParameter, IPatient, IInfluence> patientData = null;
                    int id = int.Parse(row[0]);
                    if (isDynamicRows)
                        patientData = patientParameters[id];
                    else
                    {
                        patientData = new PatientData()
                        {
                            PatientId = id,
                            Timestamp = DateTime.MinValue, //TODO указывать во входных данных
                            Influence = influence                 
                        };
                        patientData.Patient = new Patient()
                        {
                            MedicalHistoryNumber = id,
                            Name = "",
                            Birthday = DateTime.MinValue
                        };
                        patientData.Influence.PatientId = id;
                        patientParameters[id] = patientData;
                    }

                    
                    Parallel.For(1, row.Count, j =>
                    {
                        try
                        {
                            if (headerParamsNames[j] == ParameterNames.None)
                                return;
                            ParameterNames parameterName = headerParamsNames[j]; //Доступ к общему листу problem

                            if (!patientData.Parameters.ContainsKey(parameterName))
                            {
                                patientData.Parameters[parameterName] = new PatientParameter(parameterName)
                                {
                                    Timestamp = DateTime.MinValue, //TODO  нужно указывать во входных данных.
                                    PatientId = id,
                                    PositiveDynamicCoef = 1 //TODO нужно указывать во входных данных.
                                };
                            };

                            if (isDynamicRows)
                                patientData.Parameters[parameterName].DynamicValue = row[j];
                            else
                                patientData.Parameters[parameterName].Value = row[j];
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
