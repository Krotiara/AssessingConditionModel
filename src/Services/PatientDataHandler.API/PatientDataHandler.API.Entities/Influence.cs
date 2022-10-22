using Interfaces;
using System.Collections.Concurrent;

namespace PatientDataHandler.API.Entities
{
    public class Influence : IInfluence<Patient, PatientParameter>
    {

        public Influence() 
        {
            StartParameters = new ConcurrentDictionary<ParameterNames, PatientParameter>();
            DynamicParameters = new ConcurrentDictionary<ParameterNames, PatientParameter>();
        }

        public int Id { get ; set ; }
        public DateTime StartTimestamp { get ; set ; }
        public DateTime EndTimestamp { get ; set ; }
        public InfluenceTypes InfluenceType { get ; set ; }
        public string MedicineName { get ; set ; }
        public int PatientId { get; set; }
        public ConcurrentDictionary<ParameterNames, PatientParameter> StartParameters { get; set; }
        public ConcurrentDictionary<ParameterNames, PatientParameter> DynamicParameters { get; set; }
        public Patient Patient { get; set; }
    }
}
