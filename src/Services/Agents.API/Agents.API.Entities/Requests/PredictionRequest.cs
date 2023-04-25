using Agents.API.Entities.AgentsSettings;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public class PredictionRequest
    {
        public string Id { get; set; }

        public string Affiliation { get; set; }

        public Dictionary<CommonProperties, Property> CommonProperties { get; set; }

        public List<PredictionSettings> Settings { get; set; }
    }
}
