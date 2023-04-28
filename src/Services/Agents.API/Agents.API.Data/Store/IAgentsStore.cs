using Agents.API.Entities.AgentsSettings;
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
        public IAgent GetAgent(IAgentKey key, AgentsSettings settings);

        public void Clear();
    }
}
