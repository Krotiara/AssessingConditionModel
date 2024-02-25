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
using ASMLib.Entities;

namespace Agents.API.Entities.Documents
{
    public class AgentSettings : IAgentSettings
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

        [JsonIgnore]
        public AgentPropertiesNamesSettings CommonNamesSettings { get; set; }
    }
}

