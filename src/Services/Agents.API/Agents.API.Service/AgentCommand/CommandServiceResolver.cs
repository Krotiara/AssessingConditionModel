using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public delegate IAgentCommand CommandServiceResolver(SystemCommands command, 
        ConcurrentDictionary<string, IProperty> vars
        ConcurrentDictionary<string, IProperty> properties);
}
