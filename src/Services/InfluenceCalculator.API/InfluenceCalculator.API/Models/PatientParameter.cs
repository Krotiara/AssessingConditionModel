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
            NameTextDescription = name;
            Value = value;
            PositiveDynamicCoef = positiveDynamicCoef;
            DynamicValue = dynamicValue;
        }

        public int Id { get; set; }

        public int InfluenceId { get; set; }

        public string NameTextDescription { get; set; }
        public string Value { get; set; }
        public string? DynamicValue { get; set; }

        public int PositiveDynamicCoef { get; set; } = 1;
        public int PatientId { get; set; }
        public DateTime Timestamp { get; set; }
        public ParameterNames ParameterName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); } //TODO по имени получить Parameters.
        public bool IsDynamic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
