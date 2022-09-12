using Interfaces;

namespace PatientDataHandler.API.Models
{
    public class PatientData : IPatientData
    {
        public PatientData()
        {

        }

        public int PatientId { get ; set ; }
        public int InfluenceId { get ; set ; }
        public IInfluence Influence { get ; set ; }
        public IEnumerable<IPatientParameter> Parameters { get ; set ; }
    }
}
