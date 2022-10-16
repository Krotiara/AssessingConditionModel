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
        public Task<double> Handle(CalculateBioAgeByFunctionalParamsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
