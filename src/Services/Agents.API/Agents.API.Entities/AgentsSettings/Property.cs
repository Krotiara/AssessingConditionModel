using Agents.API.Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.AgentsSettings
{
    public class Property : IProperty
    {
        public Property() { }

        public Property(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public Property(object value, string name, string type)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public object Value { get; set; }
    }
}

