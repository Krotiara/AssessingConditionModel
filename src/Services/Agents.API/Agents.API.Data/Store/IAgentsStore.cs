using Agents.API.Entities.Documents;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Store
{
    public interface IAgentsStore
    {
        public IAgent GetAgent(IAgentKey key, AgentSettings settings);

        public void Clear();
    }
}
