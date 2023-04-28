using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.AgentsSettings
{
    public class PredictionSettings
    {
        public string SettingsName { get; set; }

        public List<Property> Variables { get; set; }
    }
}
