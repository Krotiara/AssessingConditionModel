using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class AgentState : Interfaces.DynamicAgent.IAgentState
    {
        public AgentState(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
