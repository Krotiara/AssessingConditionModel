using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class AgingDynamics : IAgingDynamics<AgingState>
    {
        public int PatientId { get ; set ; }
        public InfluenceTypes InfluenceType { get ; set ; }
        public DateTime StartTimestamp { get ; set ; }
        public DateTime EndTimestamp { get ; set ; }
        public string MedicineName { get ; set ; }
        public AgingState AgentStateInInfluenceStart { get ; set ; }
        public AgingState AgentStateInInfluenceEnd { get ; set ; }
    }
}
