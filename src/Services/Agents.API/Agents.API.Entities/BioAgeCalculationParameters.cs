﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class BioAgeCalculationParameters : IBioAgeCalculationParameters<PatientParameter>
    {
        public BioAgeCalculationType CalculationType { get ; set ; }
        public Dictionary<string, PatientParameter> Parameters { get ; set ; }
    }
}
