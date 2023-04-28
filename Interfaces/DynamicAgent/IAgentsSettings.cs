using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{
    public interface IAgentsSettings
    {
        public AgentType AgentType { get; set; }

        public List<IProperty> StateProperties { get; set; }

        public List<IProperty> Variables { get; set; }

        public string StateResolveCode { get; set; }

        public List<IAgentState> States { get; set; }
    }
}
