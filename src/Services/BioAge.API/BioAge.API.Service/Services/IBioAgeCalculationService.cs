using BioAge.API.Entites;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioAge.API.Service.Services
{
    public interface IBioAgeCalculationService
    {
        public Task<double> CalculateBioAge(BioAgeCalculationParameters calcParameters);
    }
}
