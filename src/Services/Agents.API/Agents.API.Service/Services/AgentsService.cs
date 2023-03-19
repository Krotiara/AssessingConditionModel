using Agents.API.Data.Repository;
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
    public class AgentsService : IAgentsService
    {
        private readonly IAgentInitSettingsProvider _agentInitSettingsProvider;
        private readonly IDynamicAgentsRepository _dynamicAgentsRepository;

        public AgentsService(IAgentInitSettingsProvider agentInitSettingsProvider, IDynamicAgentsRepository dynamicAgentsRepository)
        {
            _agentInitSettingsProvider = agentInitSettingsProvider;
            _dynamicAgentsRepository = dynamicAgentsRepository;
        }

        public IDynamicAgent InitAgentBy(IAgentKey key, AgentType agentType)
        {
            IDynamicAgentInitSettings initSets = _agentInitSettingsProvider.GetSettingsBy(agentType);
            return _dynamicAgentsRepository.InitAgent(key, initSets);
        }

        public IDynamicAgent InitAgentBy(IAgentKey key, IDynamicAgentInitSettings settings)
        {
            return _dynamicAgentsRepository.InitAgent(key, settings);
        }
    }
}
