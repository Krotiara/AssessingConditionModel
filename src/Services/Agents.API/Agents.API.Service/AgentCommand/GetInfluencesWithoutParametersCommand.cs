using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public class GetInfluencesWithoutParametersCommand : IAgentCommand
    {
        private readonly IWebRequester _webRequester;
        public GetInfluencesWithoutParametersCommand(IWebRequester webRequester)
        {
            _webRequester = webRequester;
        }

        public Delegate Command => async (DateTime startTimestamp, DateTime endTimestamp, int patientId, string medOrganization) =>
        {
            PatientInfluencesRequest request = new PatientInfluencesRequest()
            {
                PatientId = patientId,
                MedicalOrganization = medOrganization,
                StartTimestamp = startTimestamp,
                EndTimestamp = endTimestamp
            };
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            string url = $"{_patientsResolverApiUrl}/patientsApi/influencesWithoutParams";
            return await _webRequester.GetResponse<IList<Influence>>(url, "POST", body);
        };
    }
}
