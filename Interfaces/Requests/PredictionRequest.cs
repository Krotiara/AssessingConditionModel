using ASMLib.DynamicAgent;
using ASMLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.Requests
{
    public class PredictionRequest
    {
        public string Id { get; set; }

        public string ObservedId { get; set; }

        public string Affiliation { get; set; }

        public string AgentType { get; set; }

        public PredictionSettings Settings { get; set; }

        public IAgentSettings AgentSettings { get; set; }
    }
}
