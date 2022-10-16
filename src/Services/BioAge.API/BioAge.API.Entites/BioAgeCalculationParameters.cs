using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioAge.API.Entites
{
    public class BioAgeCalculationParameters : IBioAgeCalculationParameters<PatientParameter>
    {

        public BioAgeCalculationParameters() { }

        public BioAgeCalculationType CalculationType { get ; set ; }
        public Dictionary<ParameterNames, PatientParameter> Parameters { get ; set ; }
    }
}
