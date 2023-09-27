using PatientsResolver.API.Entities;

namespace PatientsResolver.API.Models.Requests
{
    public class PatientParameters
    {
        public string PatientId { get; set; }

        public string PatientAffiliation { get; set; }

        public IEnumerable<Parameter> Parameters { get; set; }
    }

    public class AddPatientParametersRequest
    {
        public List<PatientParameters> PatientsParameters { get; set; }

    }
}
