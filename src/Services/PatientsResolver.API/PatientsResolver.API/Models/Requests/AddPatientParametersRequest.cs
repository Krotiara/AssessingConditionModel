using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Models.Requests
{
    public class AddPatientParametersRequest
    {
        public string PatientId { get; set; }

        public string PatientAffiliation { get; set; }

        public IEnumerable<Parameter> Parameters { get; set; }

    }
}
