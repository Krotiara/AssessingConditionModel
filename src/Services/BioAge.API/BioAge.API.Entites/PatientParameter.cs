using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioAge.API.Entites
{
    public class PatientParameter : IPatientParameter
    {

        public PatientParameter() { }

        public int Id { get ; set ; }
        public int PatientDataId { get ; set ; }
        public int PatientId { get ; set ; }
        public DateTime Timestamp { get ; set ; }
        public ParameterNames ParameterName { get ; set ; }
        public string NameTextDescription { get ; set ; }
        public string Value { get ; set ; }
        public string DynamicValue { get ; set ; }
        public int PositiveDynamicCoef { get ; set ; }
    }
}
