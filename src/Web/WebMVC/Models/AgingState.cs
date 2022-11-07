using Interfaces;

namespace WebMVC.Models
{
    public class AgingState : IAgingState
    {
        public int Id { get ; set ; }
        public int PatientId { get ; set ; }
        public DateTime Timestamp { get ; set ; }
        public double Age { get ; set ; }
        public double BioAge { get ; set ; }
        public AgentBioAgeStates BioAgeState { get ; set ; }
    }
}
