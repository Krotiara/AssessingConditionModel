using Interfaces.DynamicAgent;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public class GetAgentStateRequest
    {
        public IAgentKey Key { get; set; }

        public AgentsSettings.AgentsSettings AgentsSettings { get; set; }

        public IEnumerable<IProperty> Variables { get; set; }
    }
}
