using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public interface IAgentDetermineStateProperties
    {
        public DateTime? StartTimestamp { get; set; }

        public DateTime? EndTimestamp { get; set; }
    }
}
