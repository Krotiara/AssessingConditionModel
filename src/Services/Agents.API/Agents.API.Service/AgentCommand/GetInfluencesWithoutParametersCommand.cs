using Agents.API.Entities;
using Agents.API.Entities.Requests;
using Interfaces;
using Interfaces.DynamicAgent;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
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

        public ConcurrentDictionary<string, IProperty> Variables { get; set; }
        public ConcurrentDictionary<string, IProperty> Properties { get; set; }
        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }

        public GetInfluencesWithoutParametersCommand(IWebRequester webRequester, IOptions<EnvSettings> settings)
        {
            _webRequester = webRequester;
            _patientsResolverApiUrl = settings.Value.PatientsResolverApiUrl;
        }

        public Delegate Command => async () =>
        {
            string patientId = Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Properties[PropertiesNamesSettings.Affiliation].Value as string;
            DateTime startTimestamp = (DateTime)Variables[PropertiesNamesSettings.StartTimestamp].Value;
            DateTime endTimestamp = (DateTime)Variables[PropertiesNamesSettings.EndTimestamp].Value;

#warning patientId нужно полность преобразовать в string на бэкэ
            PatientInfluencesRequest request = new PatientInfluencesRequest()
            {
                PatientId = int.Parse(patientId),
                MedicalOrganization = patientAffiliation,
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
