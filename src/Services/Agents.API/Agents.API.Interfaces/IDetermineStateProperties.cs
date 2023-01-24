using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{
    public interface IAgentDetermineStateProperties
    {
        public Dictionary<string, IAgentProperty> Properties { get; }
    }
}
