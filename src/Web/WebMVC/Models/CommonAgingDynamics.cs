using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class CommonAgingDynamics
    {
        public CommonAgingDynamics(IList<AgingDynamics> agingDynamics, 
            DateTime startTimestamp, DateTime endTimestamp)
        {
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
            AgingDynamics = agingDynamics;
            BioAgeDeltas = AgingDynamics
                .Select(x => x.AgentStateInInfluenceEnd.BioAge - x.AgentStateInInfluenceStart.BioAge)
                .ToList();
        }

        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }

        public IList<AgingDynamics> AgingDynamics { get; }

        private IList<double> BioAgeDeltas { get; }

        public double AverageBioAgeDelta => BioAgeDeltas.Average();

        public double MaxBioAgeDelta => BioAgeDeltas.Max();

    }
}
