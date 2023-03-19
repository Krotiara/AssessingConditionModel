using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAgentKey
    {
        public int ObservedId { get; set; }

        public AgentType AgentType { get; set; }

        public string ObservedObjectAffilation { get; set; }
    }
}
