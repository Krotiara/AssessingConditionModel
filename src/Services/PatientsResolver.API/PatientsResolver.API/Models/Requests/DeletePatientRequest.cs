namespace PatientsResolver.API.Models.Requests
{
    public class DeletePatientRequest
    {
        public string Affiliation { get; set; }

        public string PatientId { get; set; }
    }
}
