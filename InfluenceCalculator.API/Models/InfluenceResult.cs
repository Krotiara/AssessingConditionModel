﻿using Interfaces;
using System.Collections.Generic;

namespace InfluenceCalculator.API.Models
{
    public class InfluenceResult : IInfluenceResult
    {
        public string InfluenceName { get; set; }
        
        public IEnumerable<IPatientParameter> TrackedParameters { get; set; }

        public double GetInfluenceEffectiveness()
        {
            throw new System.NotImplementedException();
        }
    }
}