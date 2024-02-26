using ASMLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Requests
{
    public interface IInitAgentsRequest
    {

        public AgentType AgentType { get; }

        public List<AgentKey> AgentsToInit { get; }
    }
}
