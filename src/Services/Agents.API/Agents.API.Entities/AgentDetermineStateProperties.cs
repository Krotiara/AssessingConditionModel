

using Agents.API.Interfaces;

namespace Agents.API.Entities
{
    public class AgentDetermineStateProperties : IAgentDetermineStateProperties
    {
        public AgentDetermineStateProperties()
        {
            Timestamp = DateTime.MaxValue; //по-умолчанию
        }

        public DateTime Timestamp { get ; set ; }

        public bool IsNeedRecalculation { get; set; }
    }
}
