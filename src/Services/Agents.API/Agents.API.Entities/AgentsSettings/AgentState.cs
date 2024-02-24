using ASMLib.DynamicAgent;

namespace Agents.API.Entities.AgentsSettings
{
    public class AgentState : IAgentState
    {
        public string Name { get; set; }

        public double NumericCharacteristic { get; set; }

        public DateTime Timestamp { get; set; }

        public string DefinitionCode { get; set; }

        public string Description { get; set; }
    }
}
