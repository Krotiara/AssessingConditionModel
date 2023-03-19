using Interfaces;
using Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public class InitAgentsRequest: IInitAgentsRequest
    {
        public InitAgentsRequest() { }

        public List<(IAgentKey, AgentType)> AgentsToInit { get; }
    }
}
