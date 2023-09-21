using Agents.API.Entities;
using Agents.API.Entities.Documents;
using Agents.API.Entities.Requests;
using Agents.API.Service.Services;
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
    public class GetInfluencesCommand : IAgentCommand
    {
        private readonly PatientsService _requestService;

        public ConcurrentDictionary<string, IProperty> Variables { get; set; }
        public ConcurrentDictionary<string, IProperty> Properties { get; set; }
        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }

        public GetInfluencesCommand(PatientsService requestService)
        {
            _requestService = requestService;
        }

        public Delegate Command => async () =>
        {
            string patientId = Properties[PropertiesNamesSettings.Id].Value as string;
            string patientAffiliation = Properties[PropertiesNamesSettings.Affiliation].Value as string;
            DateTime startTimestamp = (DateTime)Variables[PropertiesNamesSettings.StartTimestamp].Value;
            DateTime endTimestamp = (DateTime)Variables[PropertiesNamesSettings.EndTimestamp].Value;

            PatientInfluencesRequest request = new()
            {
                PatientId = patientId,
                Affiliation = patientAffiliation,
                StartTimestamp = startTimestamp,
                EndTimestamp = endTimestamp
            };

            var influences = await _requestService.GetInfluences(request);

            if (influences == null)
                throw new ExecuteCommandException($"Cannot get influences for {request.PatientId}({request.Affiliation}).");

            return influences;
        };
    }
}
