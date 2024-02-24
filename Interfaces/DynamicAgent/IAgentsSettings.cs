using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.DynamicAgent
{
    public interface IAgentsSettings
    {
        public string Affiliation { get; set; }

        public string AgentType { get; set; }

        public List<IProperty> StateProperties { get; set; }

        public List<IProperty> Variables { get; set; }

        public string StateResolveCode { get; set; }

        public List<IAgentState> States { get; set; }

        public IAgentPropertiesNamesSettings CommonNamesSettings { get; set; }
    }
}
