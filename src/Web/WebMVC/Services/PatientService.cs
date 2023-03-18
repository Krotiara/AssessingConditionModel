using Interfaces;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class PatientService : IPatientService
    {
        private readonly IWebRequester webRequester;
        private readonly string gatewayUrl;

        public PatientService(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
            gatewayUrl = Environment.GetEnvironmentVariable("API_URl");
        }

        public async Task<bool> AddPatientsInluenceData(byte[] data)
        {
            string url = $"{gatewayUrl}/patientsApi/addInfluenceData/";
            FileData fD = new FileData() { RawData = data, MedicalOrganization = "test" };
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(fD);
            return await webRequester.GetResponse<bool>(url, "POST", body);
        }

        public async Task<bool> AddPatient(Patient p)
        {
            string url = $"{gatewayUrl}/patientsApi/addPatient";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            return await webRequester.GetResponse<bool>(url, "POST", body);
        }




        public async Task<Patient> GetPatient(int id)
        {
            try
            {
                string url = $"{gatewayUrl}/patientsApi/patients/{id}";
                return await webRequester.GetResponse<Patient>(url, "GET");
            }
            catch(GetWebResponceException ex)
            {
                throw;
            }
        }

        public async Task<IList<AgingDynamics>> GetPatientAgingDynamics(int patientId, 
            DateTime startTimestamp, DateTime endTimestamp)
        {
            try
            {
                string url = $"{gatewayUrl}/agents/agingDynamics/{patientId}";
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new DateTime[2] { startTimestamp, endTimestamp });
                return await webRequester.GetResponse<IList<AgingDynamics>>(url, "POST", body);
            }
            catch(GetWebResponceException ex)
            {
                throw;
            }
        }


        public async Task<IList<AgingDynamics>> GetAgingDynamics(DateTime startTimestamp, DateTime endTimestamp)
        {
            try
            {
                string url = $"{gatewayUrl}/agents/agingDynamics/";
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new DateTime[2] { startTimestamp, endTimestamp });
                return await webRequester.GetResponse<IList<AgingDynamics>>(url, "POST", body);
            }
            catch(GetWebResponceException ex)
            {
                throw;
            }
        }


        public async Task<AgingState> GetPatientCurrentAgingState(int patientId)
        {
            try
            {
                string url = $"{gatewayUrl}/agents/agingState/{patientId}";
                return await webRequester.GetResponse<AgingState>(url, "GET");
            }
            catch(GetWebResponceException)
            {
                return null;
            }
        }

        public async Task<bool> EditPatient(Patient p)
        {
            string url = $"{gatewayUrl}/patientsApi/updatePatient";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            return await webRequester.GetResponse<bool>(url, "PUT", body);
        }

        public async Task<IList<Influence>> GetPatientInfluences(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
            string url = $"{gatewayUrl}/patientsApi/influences/{patientId}";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(
                    new DateTime[2] { startTimestamp, endTimestamp });
            return await webRequester.GetResponse<IList<Influence>>(url, "POST", body);
        }

        public async Task<bool> AddInfluence(Influence influence)
        {
            string url = $"{gatewayUrl}/patientsApi/influence/add";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(influence);
            return await webRequester.GetResponse<bool>(url, "POST", body);

        }
    }
}
