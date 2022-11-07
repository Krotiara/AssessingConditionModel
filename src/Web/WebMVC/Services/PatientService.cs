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

        public async Task<bool> AddPatientsInluenceData(byte[] data)
        {
            string url = $"https://host.docker.internal:8009/addInfluenceData/";
            FileData fD = new FileData() { RawData = data };
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(fD);
            return await webRequester.GetResponse<bool>(url, "POST", body);
        }

        

        public async Task<Patient> GetPatient(int id)
        {
            try
            {
                string url = $"https://host.docker.internal:8009/patients/{id}";
                return await webRequester.GetResponse<Patient>(url, "GET");
            }
            catch(GetWebResponceException ex)
            {
                return null; //TODO log
            }
        }

        public async Task<IList<AgingDynamics>> GetPatientAgingDynamics(int patientId, 
            DateTime startTimestamp, DateTime endTimestamp)
        {
            string url = $"https://host.docker.internal:8009/agingDynamics/{patientId}";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(
                new DateTime[2] { startTimestamp, endTimestamp});
            return await webRequester.GetResponse<IList<AgingDynamics>>(url, "POST", body);
        }


        public async Task<IList<AgingDynamics>> GetAgingDynamics(DateTime startTimestamp, DateTime endTimestamp)
        {
            string url = $"https://host.docker.internal:8009/agingDynamics/";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(
                new DateTime[2] { startTimestamp, endTimestamp });
            return await webRequester.GetResponse<IList<AgingDynamics>>(url, "POST", body);
        }


        public async Task<AgingState> GetPatientCurrentAgingState(int patientId)
        {
            string url = $"https://host.docker.internal:8009/agingState/{patientId}";
            return await webRequester.GetResponse<AgingState>(url, "GET");
        }
    }
}
