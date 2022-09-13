using Interfaces;

namespace PatientDataHandler.API.Models
{
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter()
        {

        }

        public int Id { get; set; }

        public int PatientId { get ; set ; }
        public DateTime Timestamp { get ; set ; }
        public string Name { get ; set ; }
        public string Value { get ; set ; }
        public string DynamicValue { get ; set ; }
        public int PositiveDynamicCoef { get ; set ; }
    }
}
