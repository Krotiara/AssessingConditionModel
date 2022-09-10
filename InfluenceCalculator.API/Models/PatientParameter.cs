using Interfaces;

namespace InfluenceCalculator.API.Models
{
    public class PatientParameter : IPatientParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public object DynamicValue { get; set; }
        public int PositiveDynamicCoef { get; set; } = 1;
    }
}
