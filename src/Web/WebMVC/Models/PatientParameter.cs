using Interfaces;

namespace WebMVC.Models
{
    public class PatientParameter : IPatientParameter
    {
        public PatientParameter() { }

        public int Id { get; set; }
        public int InfluenceId { get; set; }
        public int PatientId { get; set; }
        public DateTime Timestamp { get; set; }
        public ParameterNames ParameterName { get; set; }
        public string NameTextDescription { get; set; }
        public string Value { get; set; }
        public bool IsDynamic { get; set; }
        public int PositiveDynamicCoef { get; set; }
    }
}
