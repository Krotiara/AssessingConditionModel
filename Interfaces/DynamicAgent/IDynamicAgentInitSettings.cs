using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{
    public interface IDynamicAgentInitSettings
    {

        Dictionary<string, IProperty> Properties { get; set; }

        Dictionary<string, IAgentState> States { get; set; }

        public string DetermineAgentPropertiesActions { get; set; }
    }
}
