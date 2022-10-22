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

        public Task<IList<IAgingPatientState>> GetAgingDynamicsByPatientId(int patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<IAgingPatientState> GetAgingPatientStateByPatientId(int patientId)
        {
#warning Выскакивает ошибка запроса
            string url = $"https://host.docker.internal:8012/agingState/{patientId}";
            return await webRequester.GetResponse<AgingPatientState>(url, "GET");
        }

        public async Task<IPatient> GetPatientById(int id)
        {
            string url = $"https://host.docker.internal:8004/patients/{id}";
            return await webRequester.GetResponse<Patient>(url, "GET");
        }
    }
}
