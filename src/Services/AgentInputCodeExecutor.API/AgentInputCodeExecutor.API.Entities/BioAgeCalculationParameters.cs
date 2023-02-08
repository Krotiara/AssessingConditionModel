using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class BioAgeCalculationParameters : IBioAgeCalculationParameters<PatientParameter>
    {
        public BioAgeCalculationType CalculationType { get ; set ; }
        public Dictionary<ParameterNames, PatientParameter> Parameters { get ; set ; }
    }

}
