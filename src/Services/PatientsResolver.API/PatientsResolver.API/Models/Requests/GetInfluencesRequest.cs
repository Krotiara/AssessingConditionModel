namespace PatientsResolver.API.Models.Requests
{
    public class GetInfluencesRequest
    {
        public string PatientId { get; set; }

        public string Affiliation { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public DateTime? EndTimestamp { get; set; }
    }
}
