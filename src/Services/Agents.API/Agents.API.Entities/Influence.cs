using Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class Influence : IInfluence<Patient,PatientParameter>
    {
        public Influence() 
        {
            StartParameters = new ConcurrentDictionary<string, PatientParameter>();
            DynamicParameters = new ConcurrentDictionary<string, PatientParameter>();
        }
        public int Id { get ; set ; }
        public int PatientId { get ; set ; }
        public DateTime StartTimestamp { get ; set ; }
        public DateTime EndTimestamp { get ; set ; }
        public InfluenceTypes InfluenceType { get ; set ; }
        public string MedicineName { get ; set ; }
        public Patient Patient { get ; set ; }
        public ConcurrentDictionary<string, PatientParameter> StartParameters { get ; set ; }
        public ConcurrentDictionary<string, PatientParameter> DynamicParameters { get ; set ; }
        public string MedicalOrganization { get; set; }
    }
}
