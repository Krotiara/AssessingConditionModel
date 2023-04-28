using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.AgentsSettings
{
    public class PredictionModel
    {
        public PredictionModel() { }

        public string Organization { get; set; }

        public AgentsSettings Settings { get; set; }
    }
}
