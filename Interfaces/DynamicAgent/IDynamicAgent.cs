using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{
    public interface IDynamicAgent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Dictionary<ParameterNames, IProperty> Properties { get; }

        public IStateDiagram StateDiagram { get; set; }
    }
}
