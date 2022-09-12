using Interfaces;

namespace PatientDataHandler.API.Models
{
    public interface IDataProvider
    {
        public IList<IPatientData> ParseData(string filePath);
    }
}
