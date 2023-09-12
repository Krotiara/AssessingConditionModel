namespace PatientsResolver.API.Models.Requests
{
    public class GetPatientParametersRequest
    {
        public string Affiliation { get; set; }

        public string PatientId { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public DateTime? EndTimestamp { get; set; }

        public List<string> Names { get; set; }
    }
}
