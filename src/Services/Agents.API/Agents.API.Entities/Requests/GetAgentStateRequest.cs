using Interfaces.DynamicAgent;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agents.API.Entities.Documents;

namespace Agents.API.Entities.Requests
{
    public class GetAgentStateRequest
    {
        public GetAgentStateRequest(IAgentKey key, AgentSettings agentsSettings, IEnumerable<IProperty> variables)
        {
            Key = key;
            AgentsSettings = agentsSettings;
            Variables = variables;
        }

        public IAgentKey Key { get; set; }

        public AgentSettings AgentsSettings { get; set; }

        public IEnumerable<IProperty> Variables { get; set; }
    }
}
