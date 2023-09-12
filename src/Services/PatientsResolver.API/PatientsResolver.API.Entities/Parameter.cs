using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Entities
{
    public struct Parameter
    {
        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        public double Value { get; set; }
    }
}
