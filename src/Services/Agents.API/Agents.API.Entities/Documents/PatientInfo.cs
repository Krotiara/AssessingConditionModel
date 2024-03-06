using PatientsResolver.API.Entities;

namespace Agents.API.Entities.Documents
{
    public class PatientInfo 
    {
        public Patient Patient { get; set; }

        public PatientMeta Meta { get; set; }
    }
}
