﻿using ASMLib.DynamicAgent;
using ASMLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests.Responce
{
    public class GetAgentStateResponce
    {
        public AgentState AgentState { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsError => ErrorMessage != null && ErrorMessage != string.Empty;
    }
}
