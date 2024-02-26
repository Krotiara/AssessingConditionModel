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
using ASMLib.Entities;

namespace Agents.API.Data.Store
{
    public class MemoryAgentsStore : IAgentsStore
    {
        private ConcurrentDictionary<AgentKey, IAgent> _agents;
        private readonly ICodeExecutor _codeExecutor;

        public MemoryAgentsStore(ICodeExecutor codeExecutor)
        {
            _agents = new ConcurrentDictionary<AgentKey, IAgent>();
            _codeExecutor = codeExecutor;
        }


        public Task<IAgent> Get(AgentKey key)
        {
            if (!_agents.ContainsKey(key))
                _agents[key] = new Agent(key, _codeExecutor);
            
            return Task.FromResult(_agents[key]);
        }

        public Task Clear()
        {
            _agents.Clear();
            return Task.CompletedTask;
        }
        
    }
}
