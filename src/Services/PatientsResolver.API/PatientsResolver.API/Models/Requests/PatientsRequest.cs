using Interfaces;

namespace PatientsResolver.API.Models.Requests
{
    public class PatientsRequest
    {
        public string Affiliation { get; set; }

        public GenderEnum? Gender { get; set; }  
    }
}
