

namespace Agents.API.Entities
{
    public class AgentDetermineStateProperties : IAgentDetermineStateProperties
    {
        public DateTime? StartTimestamp { get ; set ; }
        public DateTime? EndTimestamp { get ; set ; }
    }
}
