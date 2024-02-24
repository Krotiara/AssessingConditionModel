using Interfaces;
using ASMLib.DynamicAgent;

namespace Agents.API.Service.AgentCommand
{
    public delegate IAgentCommand CommandServiceResolver(SystemCommands command, 
        IAgent agent,
        IAgentPropertiesNamesSettings commonPropertiesNames);
}
