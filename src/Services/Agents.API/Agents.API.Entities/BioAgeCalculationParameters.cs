using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    internal class BioAgeCalculationParameters : IBioAgeCalculationParameters<PatientParameter>
    {
        public BioAgeCalculationType CalculationType { get ; set ; }
        public Dictionary<ParameterNames, PatientParameter> Parameters { get ; set ; }
    }
}
