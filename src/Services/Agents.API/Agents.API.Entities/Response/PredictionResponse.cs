using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Response
{
    public class StatePredictions
    {
        public StatePredictions(string id, string affiliation)
        {
            Id = id;
            Affiliation = affiliation;
            Predictions = new List<StatePrediction>();
        }

        public string Id { get; set; }

        public string Affiliation { get; set; }

        public List<StatePrediction> Predictions { get; set; }
    }
}
