using Interfaces;
using ASMLib.DynamicAgent;
using ASMLib.Entities;

namespace Agents.API.Service.AgentCommand
{
    public interface IAgentCommand
    {
        public Delegate Command { get; }

        public IAgent Agent { get; set; }

        public AgentPropertiesNamesSettings PropertiesNamesSettings { get; set; }
    }
}
