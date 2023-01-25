using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces.DynamicAgent
{
    public interface IDetermineStateProperties
    {
        public Dictionary<ParameterNames, IAgentProperty> Properties { get; }
    }
}
