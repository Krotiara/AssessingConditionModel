namespace PatientsResolver.API.Models.Requests
{
    public class GetPatientRequest
    {
        public string Affiliation { get; set; }

        public string PatientId { get; set; }
    }
}
