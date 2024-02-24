using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Documents;
using Agents.API.Interfaces;
using Interfaces;
using ASMLib.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Store
{
    public class MemoryAgentsStore : IAgentsStore
    {
        private ConcurrentDictionary<IAgentKey, IAgent> _agents;
        private readonly ICodeExecutor _codeExecutor;

        public MemoryAgentsStore(ICodeExecutor codeExecutor)
        {
            _agents = new ConcurrentDictionary<IAgentKey, IAgent>();
            _codeExecutor = codeExecutor;
        }


        public Task<IAgent> GetAgent(IAgentKey key, AgentSettings settings)
        {
            if (!_agents.ContainsKey(key))
                _agents[key] = new Agent(key, settings, _codeExecutor);
            return Task.FromResult(_agents[key]);
        }

        public Task Clear()
        {
            _agents.Clear();
            return Task.CompletedTask;
        }
        
    }
}
