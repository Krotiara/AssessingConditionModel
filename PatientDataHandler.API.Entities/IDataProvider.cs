using Interfaces;

namespace PatientDataHandler.API.Entities
{
    public interface IDataProvider
    {
        public IList<IPatientData> ParseData(string filePath);

        public IList<IPatientData> ParseData(byte[] bytesData);
    }
}
