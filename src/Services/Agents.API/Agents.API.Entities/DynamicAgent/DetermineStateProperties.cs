using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class DetermineStateProperties : IDetermineStateProperties
    {
        public DetermineStateProperties(Dictionary<string, IProperty> props, DateTime timestamp)
        {
            Properties = props;
            Timestamp = timestamp;
        }

        public Dictionary<string, IProperty> Properties { get; }

        public DateTime Timestamp { get; }
    }
}
