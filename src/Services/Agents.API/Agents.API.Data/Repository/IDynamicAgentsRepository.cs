using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Repository
{
    public interface IAgentsStore
    {
        public IAgent GetAgent(IAgentKey key, IAgentsSettings settings);

        public void Clear();
    }
}
