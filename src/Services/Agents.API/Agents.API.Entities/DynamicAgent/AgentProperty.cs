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

        public AgentProperty(string originName, Type type, ParameterNames name = ParameterNames.None)
        {
            OriginName = originName;
            Name = name;
            Type = type;
        }

        public ParameterNames Name { get; set; }

        public Type Type { get; set; }

        public object Value { get; set; }
        public string OriginName { get; set; }
    }
}
