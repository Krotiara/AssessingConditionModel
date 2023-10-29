using Interfaces;

namespace PatientsResolver.API.Models.Requests
{
    public class GetPatientsRequest
    {
        public string Affiliation { get; set; }

        public GenderEnum? Gender { get; set; }

        public int? StartAge { get; set; }

        public int? EndAge { get; set; }

        public string? InfluenceName { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
    }
}
