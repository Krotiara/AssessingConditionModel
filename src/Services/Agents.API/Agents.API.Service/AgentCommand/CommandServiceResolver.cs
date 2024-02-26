using Interfaces;
using ASMLib.DynamicAgent;
using ASMLib.Entities;

namespace Agents.API.Service.AgentCommand
{
    public delegate IAgentCommand CommandServiceResolver(SystemCommands command, 
        IAgent agent,
        AgentPropertiesNamesSettings commonPropertiesNames);
}
