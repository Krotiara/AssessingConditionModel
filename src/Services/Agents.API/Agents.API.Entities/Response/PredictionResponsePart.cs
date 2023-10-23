using ASMLib.DynamicAgent;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Response
{
    public class PredictionResponsePart
    {
        public PredictionResponsePart(string name, IAgentState agentState, IEnumerable<IProperty> properties, IEnumerable<IParameter> buffer)
        {
            Name = name;
            AgentState = agentState;
            Properties = properties;
            Buffer = buffer;
        }

        public string Name { get; set; }

        public IAgentState AgentState { get; set; }

        public IEnumerable<IProperty> Properties { get; set; }

        public IEnumerable<IParameter> Buffer { get; set; }
    }
}
