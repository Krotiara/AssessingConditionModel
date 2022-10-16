using Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class PatientData : IPatientData<PatientParameter, Patient, Influence>
    {
        public PatientData() { }
        public int Id { get ; set ; }
        public DateTime Timestamp { get ; set ; }
        public Patient Patient { get ; set ; }
        public Influence Influence { get ; set ; }
        public int PatientId { get ; set ; }
        public int InfluenceId { get ; set ; }
        public ConcurrentDictionary<ParameterNames, PatientParameter> Parameters { get ; set ; }
    }
}
