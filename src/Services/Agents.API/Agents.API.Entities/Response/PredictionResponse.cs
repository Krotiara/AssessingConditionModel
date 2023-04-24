using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Response
{
    public class PredictionResponse
    {
        public PredictionResponse(string id, string affiliation)
        {
            Id = id;
            Affiliation = affiliation;
            Predictions = new List<PredictionResponsePart>();
        }

        public string Id { get; set; }

        public string Affiliation { get; set; }

        public List<PredictionResponsePart> Predictions { get; set; }
    }
}
