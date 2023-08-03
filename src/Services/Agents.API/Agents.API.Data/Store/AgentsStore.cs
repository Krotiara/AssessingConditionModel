using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Store
{
#warning Репозиторий только для одного вида агентов или для разных? Если для разных, то будет конфликт ключей observableId.
    public class AgentsStore : IAgentsStore
    {
        private ConcurrentDictionary<IAgentKey, IAgent> _agents;
        private readonly ICodeExecutor _codeExecutor;

        public AgentsStore(ICodeExecutor codeExecutor)
        {
            _agents = new ConcurrentDictionary<IAgentKey, IAgent>();
            _codeExecutor = codeExecutor;
        }


        public IAgent GetAgent(IAgentKey key, AgentSettings settings)
        {
            if (!_agents.ContainsKey(key))
                _agents[key] = new Agent(key, settings, _codeExecutor);
            return _agents[key];
        }

        public void Clear() => _agents.Clear();
    }
}
