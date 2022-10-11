using Interfaces;

namespace InfluenceCalculator.API.Models
{
    public interface IInfluenceEffectivenessCalculator
    {
        public IInfluenceResult CalculateInfluence(IPatientData patientData);

    }
}
