using Interfaces;

namespace InfluenceCalculator.API.Models
{
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter()
        {

        }

        public PatientParameter(int patientId, DateTime timestamp, string name, object value,
            int positiveDynamicCoef = 1, object? dynamicValue = null)
        {
            Name = name;
            Value = value;
            PositiveDynamicCoef = positiveDynamicCoef;
            DynamicValue = dynamicValue;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public object Value { get; set; }
        public object? DynamicValue { get; set; }

        public int PositiveDynamicCoef { get; set; } = 1;
        public int PatientId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
