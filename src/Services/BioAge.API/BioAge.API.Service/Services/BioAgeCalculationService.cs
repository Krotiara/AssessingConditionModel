using BioAge.API.Entites;
using BioAge.API.Service.Command;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioAge.API.Service.Services
{
    public class BioAgeCalculationService : IBioAgeCalculationService
    {
        private readonly IMediator mediator;

        public BioAgeCalculationService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<double> CalculateBioAge(BioAgeCalculationParameters calcParameters)
        {
            switch(calcParameters.CalculationType)
            {
                case BioAgeCalculationType.ByFunctionalParameters:
                    return await mediator.Send(
                        new CalculateBioAgeByFunctionalParamsCommand() { BioAgeCalculationParameters = calcParameters });

            }
            throw new BioAgeCalculationException($"Unresolve BioAgeCalculationType: {calcParameters.CalculationType}");
        }
    }
}
