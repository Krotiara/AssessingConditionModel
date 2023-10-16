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
        public PredictionResponsePart(string name, IAgentState agentState, IEnumerable<IProperty> properties, IEnumerable<IProperty> variables)
        {
            Name = name;
            AgentState = agentState;
            Properties = properties;
            Variables = variables;
        }

        public string Name { get; set; }

        public IAgentState AgentState { get; set; }

        public IEnumerable<IProperty> Properties { get; set; }

        public IEnumerable<IProperty> Variables { get; set; }
    }
}
