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

        public Property(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public Property(object value, string name, Type type)
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

