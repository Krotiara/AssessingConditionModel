using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAgingDynamics<T> where T: IAgingState
    {
        public int PatientId { get; set; }

        public InfluenceTypes InfluenceType { get; set; }

        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }

        public string MedicineName { get; set; }

        public T AgentStateInInfluenceStart { get; set; }

        public T AgentStateInInfluenceEnd { get; set; }
    }
}
