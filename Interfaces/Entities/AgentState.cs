using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.Entities
{
    public class AgentState
    {
        public string Name { get; set; }

        public double NumericCharacteristic { get; set; }

        public DateTime Timestamp { get; set; }

        public string DefinitionCode { get; set; }

        public string Description { get; set; }
    }
}
