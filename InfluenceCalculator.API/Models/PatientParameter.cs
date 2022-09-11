using Interfaces;

namespace InfluenceCalculator.API.Models
{
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter()
        {

        }

        public PatientParameter(string name, object value,
            int positiveDynamicCoef = 1, object? dynamicValue = null)
        {
            Name = name;
            Value = value;
            PositiveDynamicCoef = positiveDynamicCoef;
            DynamicValue = dynamicValue;
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public object? DynamicValue { get; set; }

        public int PositiveDynamicCoef { get; set; } = 1;
        public int PatientId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
