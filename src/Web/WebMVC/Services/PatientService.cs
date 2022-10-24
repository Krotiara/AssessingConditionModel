using Interfaces;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class PatientService : IPatientService
    {
        private readonly IWebRequester webRequester;

        /*string url = $"https://host.docker.internal:8012/agingDynamics/{patientId}";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
            return await webRequester
                  .GetResponse<IList<AgingDynamics>>(url, "POST", body);
        int patientId = int.Parse(id); //TODO check valid in view
        string url = $"https://host.docker.internal:8009/patients/{patientId}";
        Patient p;*/

        public PatientService(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
        }

        public async Task<bool> AddPatientsInluenceData(string pathToFile)
        {
            string url = $"https://host.docker.internal:8009/addInfluenceData/{pathToFile}";
            return await webRequester.GetResponse<bool>(url, "POST");
        }

        public async Task<Patient> GetPatient(int id)
        {
            string url = $"https://host.docker.internal:8009/patients/{id}";
            return await webRequester.GetResponse<Patient>(url, "GET");
        }

        public async Task<IList<AgingDynamics>> GetPatientAgingDynamics(int patientId, 
            DateTime startTimestamp, DateTime endTimestamp)
        {
            string url = $"https://host.docker.internal:8009/agingDynamics/{patientId}";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(
                new DateTime[2] { startTimestamp, endTimestamp});
            return await webRequester.GetResponse<IList<AgingDynamics>>(url, "POST", body);
        }

        public async Task<AgingPatientState> GetPatientCurrentAgingState(int patientId)
        {
            string url = $"https://host.docker.internal:8009/agingState/{patientId}";
            return await webRequester.GetResponse<AgingPatientState>(url, "GET");
        }
    }
}
