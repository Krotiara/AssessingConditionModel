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
                double systolicPressure = double.Parse(paramsDict[ParameterNames.SystolicPressure].Value);
                double diastolicPressure = double.Parse(paramsDict[ParameterNames.DiastolicPressure].Value);
                return await Task.FromResult<double>(
                   -1.07 * systolicPressure
                   + 1.1 * diastolicPressure
                   + 1.94 * (systolicPressure - diastolicPressure)
                   - 1.45 * double.Parse(paramsDict[ParameterNames.InhaleBreathHolding].Value)
                   + 1.32 * double.Parse(paramsDict[ParameterNames.OuthaleBreathHolding].Value)
                   - 3.46 * double.Parse(paramsDict[ParameterNames.LungCapacity].Value)
                   + 0.15 * double.Parse(paramsDict[ParameterNames.Weight].Value)
                   - 4.35 * double.Parse(paramsDict[ParameterNames.Accommodation].Value)
                   + 5.57 * double.Parse(paramsDict[ParameterNames.HearingAcuity].Value)
                   - 2.6 * double.Parse(paramsDict[ParameterNames.StaticBalancing].Value));
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
