﻿using Agents.API.Entities.AgentsSettings;
using Interfaces;
using Interfaces.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public class InitAgentsRequest: IInitAgentsRequest
    {

        public InitAgentsRequest() { }

        public AgentType AgentType { get; set; }

        public List<AgentKey> AgentsToInit { get; set; }

        [NotMapped]
        List<IAgentKey> IInitAgentsRequest.AgentsToInit => AgentsToInit.Select(x => x as IAgentKey).ToList();
    }
}
