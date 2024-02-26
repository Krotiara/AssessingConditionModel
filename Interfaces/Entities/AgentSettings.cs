using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.Entities
{
    public class AgentSettings
    {
        public AgentSettings()
        {
            StateProperties = new();
            Variables = new();
            States = new();
        }

        public string Id { get; set; }

        public string Affiliation { get; set; }

        public string AgentType { get; set; }

        public List<Property> StateProperties { get; set; }

        public List<Property> Variables { get; set; }

        public string StateResolveCode { get; set; }

        public List<AgentState> States { get; set; }

        public AgentPropertiesNamesSettings CommonNamesSettings { get; set; }
    }
}
