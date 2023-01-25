using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class AgentProperty : IAgentProperty
    {
        public ParameterNames Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
    }
}
