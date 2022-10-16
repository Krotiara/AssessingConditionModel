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
    public class CalculateBioAgeByFunctionalParamsCommand: IRequest<double>
    {
        public BioAgeCalculationParameters BioAgeCalculationParameters { get; set; }
    }
}
