using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IPatientService
    {
        public Task<Patient> GetPatient(int id);

        public Task<AgingPatientState> GetPatientCurrentAgingState(int patientId);

        public Task<IList<AgingDynamics>> GetPatientAgingDynamics(int patientId,
            DateTime startTimestamp, DateTime endTimestamp);

        public Task<bool> AddPatientsInluenceData(byte[] data);
    }
}
