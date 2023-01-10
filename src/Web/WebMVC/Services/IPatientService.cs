using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IPatientService
    {
        public Task<Patient> GetPatient(int id);

        public Task<AgingState> GetPatientCurrentAgingState(int patientId);

        public Task<IList<AgingDynamics>> GetPatientAgingDynamics(int patientId,
            DateTime startTimestamp, DateTime endTimestamp);

        public Task<IList<Influence>> GetPatientInfluences(int patientId, 
            DateTime startTimestamp, DateTime endTimestamp);

        public Task<IList<AgingDynamics>> GetAgingDynamics(DateTime startTimestamp, 
            DateTime endTimestamp);


        public Task<bool> AddInfluence(Influence influence);

        public Task<bool> AddPatientsInluenceData(byte[] data);

        public Task<bool> AddPatient(Patient p);

        public Task<bool> EditPatient(Patient p);
    }
}
