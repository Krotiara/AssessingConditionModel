using Interfaces;

namespace WebMVC.Models
{
    public class AgingDynamics : IAgingDynamics<AgingState>
    {

        public AgingDynamics() { }

        public int PatientId { get ; set ; }
        public InfluenceTypes InfluenceType { get ; set ; }
        public DateTime StartTimestamp { get ; set ; }
        public DateTime EndTimestamp { get ; set ; }
        public string MedicineName { get ; set ; }
        public AgingState AgentStateInInfluenceStart { get ; set ; }
        public AgingState AgentStateInInfluenceEnd { get ; set ; }

        public double StartDelta => AgentStateInInfluenceStart.BioAge - AgentStateInInfluenceStart.Age;
        public double EndDelta => AgentStateInInfluenceEnd.BioAge - AgentStateInInfluenceEnd.Age;
    }
}
