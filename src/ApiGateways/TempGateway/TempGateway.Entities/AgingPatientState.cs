using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempGateway.Entities
{
    public class AgingPatientState : IAgingPatientState
    {
        public int PatientId { get ; set ; }
        public DateTime StartTimestamp { get ; set ; }
        public DateTime EndTimestamp { get ; set ; }
        public double Age { get ; set ; }
        public double BioAge { get ; set ; }
        public AgentBioAgeStates AgentBioAgeState { get ; set ; }
    }
}
