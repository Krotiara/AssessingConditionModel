using Interfaces;
using System.Collections;
using System;
using System.Collections.Generic;

namespace AssessingConditionModel.Models.InfluenceModel
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
    }
}
