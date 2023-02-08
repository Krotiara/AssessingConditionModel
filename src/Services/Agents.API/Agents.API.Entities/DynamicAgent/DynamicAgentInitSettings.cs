using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class DynamicAgentInitSettings : IDynamicAgentInitSettings
    {

        public Dictionary<string, IProperty> Properties { get; set; }

        public Dictionary<string, IAgentState> States { get; set; }

        public string DetermineAgentPropertiesActions { get; set; }
    }
}
