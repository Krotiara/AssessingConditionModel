using Interfaces;

namespace PatientDataHandler.API.Models
{
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter()
        {

        }

        public int PatientId { get ; set ; }
        public DateTime Timestamp { get ; set ; }
        public string Name { get ; set ; }
        public object Value { get ; set ; }
        public object DynamicValue { get ; set ; }
        public int PositiveDynamicCoef { get ; set ; }
    }
}
