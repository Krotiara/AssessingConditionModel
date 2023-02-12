using Agents.API.Interfaces;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class AgentInitSettings : IAgentInitSettings
    {

        public AgentInitSettings(int observedId, AgentType agentType)
        {
            ObservedId = observedId;
            AgentType = agentType;
        }

        public int ObservedId { get; }

        public AgentType AgentType { get; }
    }
}
