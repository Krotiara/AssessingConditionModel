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

        public AgentProperty(Type type, object value, string originName, ParameterNames name = ParameterNames.None)
        {
            Name = name;
            Type = type;
            Value = value;
            OriginName = originName;
        }

        public ParameterNames Name { get; set; }
        public Type Type { get; set; }

        public object Value { get; set; }

        public string OriginName { get; set; }
    }
}
