using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAgentPropertiesNamesSettings
    {
        public string StartTimestamp { get; set; }

        public string EndTimestamp { get; set; }

        public string StateNumber { get; set; }

        public string CurrentState { get; set; }

        public string Id { get; set; }

        public string Affiliation { get; set; }
    }
}
