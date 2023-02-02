using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class AgentProperty : IProperty
    {

        public AgentProperty(string originName, ParameterNames name, Type type, object value)
        {
            OriginName = originName;
            Name = name;
            Type = type;
            Value = value;
        }

        public ParameterNames Name { get; set; }

        public Type Type { get; set; }

        public object Value { get; set; }
        public string OriginName { get; set; }
    }
}
