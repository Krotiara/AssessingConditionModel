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
        public string Name { get; set; }

        public IAgentState AgentState { get; set; }
    }
}
