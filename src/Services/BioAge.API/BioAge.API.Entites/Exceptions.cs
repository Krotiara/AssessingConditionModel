using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioAge.API.Entites
{
    public class BioAgeCalculationException:Exception
    {
        public BioAgeCalculationException(string message) : base(message) { }

        public BioAgeCalculationException(string message, Exception innerException) : base(message, innerException) { } 
    }
}
