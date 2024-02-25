using ASMLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.DynamicAgent
{
    public class StatePrediction
    {
        public StatePrediction(string name, AgentState agentState,
            Property[] stateProperties, Parameter[] stateBuffer)
        {
            Name = name;
            AgentState = agentState;
            StateProperties = stateProperties;
            StateBuffer = stateBuffer;
        }

        public string Name { get; set; }

        public AgentState AgentState { get; set; }

        public Property[] StateProperties { get; set; }

        public Parameter[] StateBuffer { get; set; }
    }
}
