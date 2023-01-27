using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces.DynamicAgent
{
    public interface IDynamicAgent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Dictionary<ParameterNames, IAgentProperty> Properties { get; }

        public IStateDiagram StateDiagram { get; set; }
    }
}
