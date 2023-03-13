﻿using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{
    public interface IAgentInitSettingsProvider
    {
        public IDynamicAgentInitSettings GetSettingsBy(AgentType agentType);
    }
}