using Agents.API.Entities.DynamicAgent;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Repository
{
    public interface IDynamicAgentsRepository
    {
        public IDynamicAgent GetAgent(AgentKey key);

        public IDynamicAgent InitAgent(AgentKey key, IDynamicAgentInitSettings settings);

        public void Clear();
    }
}
