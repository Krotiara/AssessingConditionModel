using Agents.API.Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class AgentProperty : IAgentProperty
    {

        public AgentProperty(string name, Type type, object value)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public string Name { get; set; }

        public Type Type { get; set; }

        public object Value { get; set; }
    }
}
