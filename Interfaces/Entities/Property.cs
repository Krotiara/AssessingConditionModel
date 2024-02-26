using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.Entities
{
    public class Property
    {
        public Property() { }

        public Property(string name, string type, object value, string description = null)
        {
            Name = name;
            Type = type;
            Value = value;
            Description = description;
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public object Value { get; set; }
    }
}
