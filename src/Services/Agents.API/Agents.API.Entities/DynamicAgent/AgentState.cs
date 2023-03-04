using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class AgentState : IAgentState
    {
        public AgentState(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public double NumericCharacteristic { get; set; }
    }
}
