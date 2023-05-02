using Interfaces;
using System.Collections.Concurrent;

namespace PatientDataHandler.API.Entities
{
    public class Influence : IInfluence<Patient, PatientParameter>
    {

        public Influence() 
        {
            StartParameters = new ConcurrentDictionary<string, PatientParameter>();
            DynamicParameters = new ConcurrentDictionary<string, PatientParameter>();
        }

        public int Id { get ; set ; }
        public DateTime StartTimestamp { get ; set ; }
        public DateTime EndTimestamp { get ; set ; }
        public string InfluenceType { get ; set ; }
        public string MedicineName { get ; set ; }
        public int PatientId { get; set; }
        public ConcurrentDictionary<string, PatientParameter> StartParameters { get; set; }
        public ConcurrentDictionary<string, PatientParameter> DynamicParameters { get; set; }
        public Patient Patient { get; set; }
        public string MedicalOrganization { get; set; }
    }
}
