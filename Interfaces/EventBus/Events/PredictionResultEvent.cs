using ASMLib.DynamicAgent;
using ASMLib.Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.EventBus.Events
{
    public class PredictionResultEvent : Event
    {
        public string PredictionId { get; set; }

        public AgentKey AgentKey { get; set; }

        public StatePrediction StatePrediction { get; set; }

        public string ErrorMessage { get; set; }
    }
}
