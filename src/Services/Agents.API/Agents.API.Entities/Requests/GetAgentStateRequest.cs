using Interfaces;
using Agents.API.Entities.Documents;
using ASMLib.Entities;
using ASMLib.DynamicAgent;

namespace Agents.API.Entities.Requests
{
    public class GetAgentStateRequest
    {
        public GetAgentStateRequest(AgentKey key, AgentSettings agentsSettings, IEnumerable<Property> variables)
        {
            Key = key;
            AgentsSettings = agentsSettings;
            Variables = variables;
        }

        public AgentKey Key { get; set; }

        public AgentSettings AgentsSettings { get; set; }

        public IEnumerable<Property> Variables { get; set; }
    }
}
