using Interfaces;
using ASMLib.DynamicAgent;

namespace Agents.API.Service.AgentCommand
{
    public interface IAgentCommand
    {
        public Delegate Command { get; }

        public IAgent Agent { get; set; }

        public IAgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }
    }
}
