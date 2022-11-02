using Interfaces;

namespace WebMVC.Models
{
    public class AgingPatientState : IAgingPatientState
    {
        public AgingPatientState() { }

        public int PatientId { get ; set ; }
        public double Age { get ; set ; }
        public double BioAge { get ; set ; }
        public AgentBioAgeStates AgentBioAgeState { get ; set ; }
    }
}
