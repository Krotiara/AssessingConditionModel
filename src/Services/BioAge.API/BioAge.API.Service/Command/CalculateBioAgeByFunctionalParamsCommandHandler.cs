using BioAge.API.Entites;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioAge.API.Service.Command
{
    public class CalculateBioAgeByFunctionalParamsCommandHandler : IRequestHandler<CalculateBioAgeByFunctionalParamsCommand, double>
    {
        public async Task<double> Handle(CalculateBioAgeByFunctionalParamsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Dictionary<ParameterNames, PatientParameter> paramsDict = request.BioAgeCalculationParameters.Parameters;
                double systolicPressure = double.Parse(
                    paramsDict[ParameterNames.SystolicPressure].Value.Replace(",","."));
                double diastolicPressure = double.Parse(
                    paramsDict[ParameterNames.DiastolicPressure].Value.Replace(",", "."));
                double inhaleBreathHolding = double.Parse(
                    paramsDict[ParameterNames.InhaleBreathHolding].Value.Replace(",", "."));
                double outhaleBreathHolding = double.Parse(
                    paramsDict[ParameterNames.OuthaleBreathHolding].Value.Replace(",", "."));
                double lungCapacity = double.Parse(
                    paramsDict[ParameterNames.LungCapacity].Value.Replace(",", "."));
                double weight = double.Parse(
                    paramsDict[ParameterNames.Weight].Value.Replace(",", "."));
                double accommodation = double.Parse(
                    paramsDict[ParameterNames.Accommodation].Value.Replace(",", "."));
                double hearingAcuity = double.Parse(
                    paramsDict[ParameterNames.HearingAcuity].Value.Replace(",", "."));
                double staticBalancing = double.Parse(
                    paramsDict[ParameterNames.StaticBalancing].Value.Replace(",", "."));
                return await Task.FromResult<double>(
                   - 1.07 * systolicPressure
                   + 1.1 * diastolicPressure
                   + 1.94 * (systolicPressure - diastolicPressure)
                   - 1.45 * inhaleBreathHolding
                   + 1.32 * outhaleBreathHolding
                   - 3.46 * lungCapacity
                   + 0.15 * weight
                   - 4.35 * accommodation
                   + 5.57 * hearingAcuity
                   - 2.6 * staticBalancing);
            }
            catch (KeyNotFoundException ex)
            {
                throw new BioAgeCalculationException("No necessary parameters in input", ex);
            }
            catch (Exception ex)
            {
                throw new BioAgeCalculationException("Unexpected exception", ex);
            }
        }
    }
}
