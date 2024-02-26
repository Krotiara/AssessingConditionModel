using ASMLib.DynamicAgent;
using ASMLib.Entities;
using ASMLib.EventBus.Events;

namespace Agents.API.Entities.Events
{
    public class PredictionResultEvent : Event
    {
        public string PredictionId { get; set; }

        public AgentKey AgentKey { get; set; }

        public StatePrediction StatePrediction { get; set; }

        public string ErrorMessage { get; set; }
    }
}
