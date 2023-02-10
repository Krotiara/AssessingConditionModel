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
        public IDynamicAgent GetAgent(int observableId);

        public IDynamicAgent InitAgent(int observableId, IDynamicAgentInitSettings settings);

        public void Clear();
    }
}
