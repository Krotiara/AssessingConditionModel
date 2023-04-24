﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.AgentsSettings
{
    public class AgentsSettings
    {
        public AgentsSettings()
        {
            StateProperties = new();
            Variables = new();
            States = new();
        }

        public AgentType AgentType { get; set; }

        public List<Property> StateProperties { get; set; }

        public List<Property> Variables { get; set; }

        public string StateResolveCode { get; set; } = "";

        public List<AgentState> States { get; set; }
    }
}

