using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Requests
{
    public interface IInitAgentsRequest
    {
        public List<(IAgentKey, AgentType)> AgentsToInit { get; }
    }
}
