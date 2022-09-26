using Interfaces;

namespace InfluenceCalculator.API.Models
{
    public class Influence : IInfluence
    {
        public int Id { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public InfluenceTypes InfluenceType { get; set; }
        public string MedicineName { get; set; }
        public int PatientId { get; set; }
    }
}
