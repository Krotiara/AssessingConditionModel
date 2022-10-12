using Interfaces;

namespace PatientDataHandler.API.Service.Services
{
    public interface IDataProvider
    {
        public IList<IPatientData<IPatientParameter, IPatient, IInfluence>> ParseData(string filePath);

        public IList<IPatientData<IPatientParameter, IPatient, IInfluence>> ParseData(byte[] bytesData);
    }
}
