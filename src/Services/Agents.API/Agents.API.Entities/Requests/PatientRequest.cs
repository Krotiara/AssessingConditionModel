using Agents.API.Entities.AgentsSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests
{
    public class PatientRequest
    {
        public string Id { get; set; }

        public PredictionModel Model { get; set; }
    }
}
