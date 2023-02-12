using Agents.API.Data.Repository;
using Agents.API.Entities;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class InitPatientAgentsService : IInitPatientAgentsService
    {

        private readonly IDynamicAgentsRepository agentPatientsRepository;
        private readonly IAgentInitSettingsProvider settingsProvider;

        public InitPatientAgentsService(IDynamicAgentsRepository agentPatientsRepository,
            IAgentInitSettingsProvider agentInitSettingsProvider)
        {
            this.agentPatientsRepository = agentPatientsRepository;
            this.settingsProvider = agentInitSettingsProvider;
        }

        public async Task<IList<IDynamicAgent>> InitAgentsAsync(IEnumerable<IAgentInitSettings> observedObjsSettings)
        {
            //TODO распараллелить
            List<string> errorMessages = new List<string>();
            IList<IDynamicAgent> agents = new List<IDynamicAgent>();

            foreach (IAgentInitSettings agentInitSets in observedObjsSettings)
            {
                try
                {
                    IDynamicAgentInitSettings settings = settingsProvider.GetSettingsBy(agentInitSets.AgentType);
                    agents.Add(agentPatientsRepository.InitAgent(agentInitSets.ObservedId, settings));
                }
                catch(InitAgentException ex)
                {
                    errorMessages.Add($"Init agent: {ex.Message}");
                    continue;
                }
            }
            if (errorMessages.Count > 0)
                throw new InitAgentsRangeException(string.Join("\n", errorMessages));
            else
                return agents;
        }
    }
}
