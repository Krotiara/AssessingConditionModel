using Interfaces;
using Agents.API.Entities.Documents;
using ASMLib.Entities;
using ASMLib.DynamicAgent;

namespace Agents.API.Entities.Requests
{
    public class GetAgentStateRequest
    {
        public GetAgentStateRequest(IAgentKey key, IAgentSettings agentsSettings, IEnumerable<Property> variables)
        {
            Key = key;
            AgentsSettings = agentsSettings;
            Variables = variables;
        }

        public IAgentKey Key { get; set; }

        public IAgentSettings AgentsSettings { get; set; }

        public IEnumerable<Property> Variables { get; set; }
    }
}
