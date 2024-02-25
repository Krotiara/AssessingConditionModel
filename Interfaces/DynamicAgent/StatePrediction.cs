using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.DynamicAgent
{
    public class StatePrediction
    {
        public StatePrediction(string name, IAgentState agentState,
            IProperty[] stateProperties, IParameter[] stateBuffer)
        {
            Name = name;
            AgentState = agentState;
            StateProperties = stateProperties;
            StateBuffer = stateBuffer;
        }

        public string Name { get; set; }

        public IAgentState AgentState { get; set; }

        public IProperty[] StateProperties { get; set; }

        public IParameter[] StateBuffer { get; set; }
    }
}
