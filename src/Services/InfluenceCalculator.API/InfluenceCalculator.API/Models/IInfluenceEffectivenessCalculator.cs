using Interfaces;

namespace InfluenceCalculator.API.Models
{
    public interface IInfluenceEffectivenessCalculator
    {
        public IInfluenceResult CalculateInfluence(IPatientData<IPatientParameter, IPatient, IInfluence> patientData);

    }
}
