using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempGateway.Entities;

namespace TempGateway.Service.Service
{
    public class PatientService : IPatientService
    {
        private IWebRequester webRequester;

        public PatientService(IWebRequester webRequester)
        {
            this.webRequester = webRequester;
        }

        public async Task<IList<AgingDynamics>> GetAgingDynamicsByPatientId(int patientId, DateTime startTimestamp, DateTime endTimestamp)
        {
            string url = $"https://host.docker.internal:8012/agingDynamics/{patientId}";
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(new DateTime[2] { startTimestamp, endTimestamp });
            return await webRequester
                  .GetResponse<IList<AgingDynamics>>(url, "POST", body);
        }

        public async Task<IAgingState> GetAgingPatientStateByPatientId(int patientId)
        {
#warning Выскакивает ошибка запроса
            string url = $"https://host.docker.internal:8012/agingState/{patientId}";
            return await webRequester.GetResponse<AgingState>(url, "GET");
        }

        public async Task<IPatient> GetPatientById(int id)
        {
            string url = $"https://host.docker.internal:8004/patients/{id}";
            return await webRequester.GetResponse<Patient>(url, "GET");
        }
    }
}
