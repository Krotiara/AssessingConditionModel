﻿using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.AgentsSettings
{
    public class AgentsSettings : IAgentsSettings
    {
        public AgentsSettings()
        {
            StateProperties = new();
            Variables = new();
            States = new();
        }

        public AgentType AgentType { get; set; }

        public List<IProperty> StateProperties { get; set; }

        public List<IProperty> Variables { get; set; }

        public string StateResolveCode { get; set; }

        public List<IAgentState> States { get; set; }
    }
}

