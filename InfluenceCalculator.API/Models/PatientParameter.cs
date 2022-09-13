using Interfaces;

namespace InfluenceCalculator.API.Models
{
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter()
        {

        }

        public PatientParameter(int patientId, DateTime timestamp, string name, string value,
            int positiveDynamicCoef = 1, string? dynamicValue = null)
        {
            Name = name;
            Value = value;
            PositiveDynamicCoef = positiveDynamicCoef;
            DynamicValue = dynamicValue;
        }

        public int Id { get; set; }

        public int PatientDataId { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
        public string? DynamicValue { get; set; }

        public int PositiveDynamicCoef { get; set; } = 1;
        public int PatientId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
