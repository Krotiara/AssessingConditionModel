using Interfaces;

namespace PatientDataHandler.API.Models
{
    public interface IDataProvider
    {
        public IPatientData ParseData(string filePath);
    }
}
