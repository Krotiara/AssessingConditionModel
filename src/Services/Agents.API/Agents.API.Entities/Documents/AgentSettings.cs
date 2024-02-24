using Agents.API.Entities.AgentsSettings;
using Interfaces;
using ASMLib.DynamicAgent;
using Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Agents.API.Entities.Documents
{
    public class AgentSettings : IAgentsSettings
    {
        public AgentSettings()
        {
            StateProperties = new();
            Variables = new();
            States = new();
        }

        public string Affiliation { get; set; }

        public string AgentType { get; set; }

        public List<IProperty> StateProperties { get; set; }


        public List<IProperty> Variables { get; set; }

        public string StateResolveCode { get; set; }

        public List<IAgentState> States { get; set; }

        [JsonIgnore]
        public IAgentPropertiesNamesSettings CommonNamesSettings { get; set; }
    }
}

