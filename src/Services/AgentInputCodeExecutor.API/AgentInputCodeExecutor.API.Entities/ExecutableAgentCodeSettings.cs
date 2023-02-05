﻿using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class ExecutableAgentCodeSettings : IExecutableAgentCodeSettings
    {
        public ExecutableAgentCodeSettings(List<string> codeLines, Dictionary<string, IProperty> properties)
        {
            CodeLines = codeLines;
            Properties = properties;
        }

        public List<string> CodeLines { get; set; }
        public Dictionary<string, IProperty> Properties { get; set; }
    }
}
