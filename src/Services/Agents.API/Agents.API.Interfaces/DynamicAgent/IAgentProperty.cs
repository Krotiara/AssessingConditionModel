using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces.DynamicAgent
{
    public interface IAgentProperty
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public object Value { get; set; }
    }
}
