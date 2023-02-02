using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces.DynamicAgent
{
    public interface IDynamicAgentInitSettings
    {
        string Name { get; }

        IEnumerable<IProperty> Properties { get; }

        IEnumerable<IAgentState> States { get; }

        Func<IDetermineStateProperties, Task<IAgentState>> DetermineStateFunc { get; }
    }
}
