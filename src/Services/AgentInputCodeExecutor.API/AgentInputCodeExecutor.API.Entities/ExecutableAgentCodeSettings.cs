using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class ExecutableAgentCodeSettings : IExecutableAgentCodeSettings<AgentProperty>
    {

        public List<string> CodeLines { get; set; }

        public Dictionary<ParameterNames, AgentProperty> Properties { get; set; }
    }
}
