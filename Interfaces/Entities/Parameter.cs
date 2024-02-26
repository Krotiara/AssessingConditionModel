using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.Entities
{
    public class Parameter
    {
        public Parameter() { }

        public Parameter(string name)
        {
            Name = name;
            Timestamp = default;
            Value = default;
        }

        public Parameter(string name, DateTime timestamp, double value)
        {
            Name = name;
            Timestamp = timestamp;
            Value = value;
        }

        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        public double Value { get; set; }
    }
}
