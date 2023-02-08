using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{

    public enum AgentType
    {
        AgingPatient
    }

    internal interface IAgentInitSettingsProvider
    {
        public IDynamicAgentInitSettings GetSettingsBy(AgentType agentType, int patientId);
    }
}
