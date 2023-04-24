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
        private readonly IAgentsStore _agentsStore;

        public AgentsService(IAgentsStore agentsStore)
        {
            _agentsStore = agentsStore;
        }

        
        //TODO - на будущее - инициализация по кастомным агентам.
        public IAgent InitAgentBy(IAgentKey key, IAgentsSettings settings)
        {
            return _agentsStore.InitAgent(key, settings);
        }
    }
}
