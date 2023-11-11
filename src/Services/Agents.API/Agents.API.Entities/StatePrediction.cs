using ASMLib.DynamicAgent;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class StatePrediction
    {
        public StatePrediction(string name, IAgentState agentState,
            IEnumerable<IProperty> stateProperties, IEnumerable<IParameter> stateBuffer)
        {
            Name = name;
            AgentState = agentState;
            StateProperties = stateProperties;
            StateBuffer = stateBuffer;
        }

        public string Name { get; set; }

        public IAgentState AgentState { get; set; }

        public IEnumerable<IProperty> StateProperties { get; set; }

        public IEnumerable<IParameter> StateBuffer { get; set; }
    }
}
