using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Documents
{
    public struct Parameter
    {
        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        public double Value { get; set; }
    }
}
