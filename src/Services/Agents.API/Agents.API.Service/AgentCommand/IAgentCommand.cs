using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.AgentCommand
{
    public interface IAgentCommand
    {
        public Delegate Command { get; }
    }
}
