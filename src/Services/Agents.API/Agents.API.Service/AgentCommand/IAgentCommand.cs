using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public interface IAgentCommand
    {
        public Delegate Command { get; }

        public ConcurrentDictionary<string, IProperty> Variables { get; set; }

        public ConcurrentDictionary<string, IProperty> Properties { get; set; }
    }
}
