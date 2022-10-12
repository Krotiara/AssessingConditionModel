using Interfaces;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PatientDataHandler.API.Entities
{
    public class PatientData : IPatientData<IPatientParameter, IPatient, IInfluence>
    {
        public PatientData()
        {
            Parameters = new ConcurrentDictionary<ParameterNames, IPatientParameter>();
        }

        
        public int Id { get; set; }

       
        public DateTime Timestamp { get; set; }

        
        public int PatientId { get ; set ; }
        
       
        public int InfluenceId { get ; set ; }
        
        //public IList<IPatientParameter> Parameters { get ; set ; }

        public IPatient Patient { get; set; }

        public IInfluence Influence { get; set; }

        public ConcurrentDictionary<ParameterNames, IPatientParameter> Parameters { get; set; }
    }
}
