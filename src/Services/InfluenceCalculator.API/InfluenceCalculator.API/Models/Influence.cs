using Interfaces;
using System.Collections.Concurrent;

namespace InfluenceCalculator.API.Models
{
    public class Influence : IInfluence<Patient, PatientParameter>
    {
        public int Id { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public InfluenceTypes InfluenceType { get; set; }
        public string MedicineName { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ConcurrentDictionary<ParameterNames, PatientParameter> StartParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ConcurrentDictionary<ParameterNames, PatientParameter> DynamicParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
