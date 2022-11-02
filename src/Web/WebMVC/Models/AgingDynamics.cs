using Interfaces;

namespace WebMVC.Models
{
    public class AgingDynamics : IAgingDynamics<AgingPatientState>
    {

        public AgingDynamics() { }

        public int PatientId { get ; set ; }
        public InfluenceTypes InfluenceType { get ; set ; }
        public DateTime StartTimestamp { get ; set ; }
        public DateTime EndTimestamp { get ; set ; }
        public string MedicineName { get ; set ; }
        public AgingPatientState AgentStateInInfluenceStart { get ; set ; }
        public AgingPatientState AgentStateInInfluenceEnd { get ; set ; }
    }
}
