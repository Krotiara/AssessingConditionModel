using Agents.API.Entities.Documents;
using Interfaces;
using ASMLib.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Store
{
    public interface IAgentsStore
    {
        public Task<IAgent> GetAgent(IAgentKey key, AgentSettings settings);

        public Task Clear();
    }
}
