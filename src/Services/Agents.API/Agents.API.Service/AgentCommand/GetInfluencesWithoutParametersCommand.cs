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
        private readonly string _patientsResolverApiUrl;
        public GetInfluencesWithoutParametersCommand(IWebRequester webRequester, EnvSettings envSettings)
        {
            _webRequester = webRequester;
            _patientsResolverApiUrl = envSettings.PatientsResolverApiUrl;
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

            var responce = await _webRequester.SendRequest(url, "POST", body);
            if (!responce.IsSuccessStatusCode)
                throw new ExecuteCommandException($"{responce.StatusCode}:{responce.ReasonPhrase}");
            else
            {
                var res = await _webRequester.DeserializeBody<IList<Influence>>(responce);
                return res;
            }
        };
    }
}
