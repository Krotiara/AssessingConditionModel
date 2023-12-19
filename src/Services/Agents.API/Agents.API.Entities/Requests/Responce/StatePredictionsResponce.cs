using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests.Responce
{
    public class StatePredictionsResponce
    {
        public StatePredictionsResponce(string id, string affiliation)
        {
            Id = id;
            Affiliation = affiliation;
        }

        public StatePredictionsResponce(string id, string affiliation, List<StatePrediction> preds) : this(id, affiliation)
        {
            Predictions = preds;
        }

        public string Id { get; set; }

        public string Affiliation { get; set; }

        public string ErrorMessage { get; set; }

        public List<StatePrediction> Predictions { get; set; }
    }
}
