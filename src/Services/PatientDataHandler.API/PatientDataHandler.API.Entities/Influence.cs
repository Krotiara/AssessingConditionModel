using Interfaces;

namespace PatientDataHandler.API.Entities
{
    public class Influence : IInfluence
    {

        public Influence() { }

        public int Id { get ; set ; }
        public DateTime StartTimestamp { get ; set ; }
        public DateTime EndTimestamp { get ; set ; }
        public InfluenceTypes InfluenceType { get ; set ; }
        public string MedicineName { get ; set ; }
        public int PatientId { get; set; }
    }
}
