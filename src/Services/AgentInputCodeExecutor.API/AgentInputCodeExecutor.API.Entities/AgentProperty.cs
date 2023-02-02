using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class AgentProperty : IProperty
    {

        public AgentProperty(ParameterNames name, Type type, object value, string originName)
        {
            Name = name;
            Type = type;
            Value = value;
            OriginName = originName;
        }

        public ParameterNames Name { get; set; } = ParameterNames.None;
        public Type Type { get; set; }

        public object Value { get; set; }

        public string OriginName { get; set; }
    }
}
