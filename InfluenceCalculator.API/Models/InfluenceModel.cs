using Interfaces;
using System.Collections;
using System;
using System.Collections.Generic;

namespace InfluenceCalculator.API.Models
{
    public class InfluenceModel
    {

        public IInfluenceResult CalculateInfluence(IPatientData inluenceDynamicData)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<IInfluenceResult> GetInfluenceHistory(int patientId)
        {
            throw new NotImplementedException();
        }


        private void SaveInfluenceResult(IInfluenceResult influenceResult)
        {
            throw new NotImplementedException();
        }
    }
}
