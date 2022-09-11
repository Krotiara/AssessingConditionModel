using Interfaces;
using System.Collections;
using System;
using System.Collections.Generic;

namespace InfluenceCalculator.API.Models
{
    public class InfluenceModel: IInfluenceEffectivenessCalculator
    {

        public IInfluenceResult CalculateInfluence(int influenceId, IPatientData influenceDynamicData)
        {
            double effectiveness = 0;
            foreach(IPatientParameter patientParameter in influenceDynamicData.Parameters)
            {
                if (patientParameter.Value.GetType() == typeof(string) 
                    || patientParameter.DynamicValue.GetType() == typeof(string))
                    continue;
                if(patientParameter.Value.GetType() == typeof(bool) 
                    && patientParameter.DynamicValue.GetType() == typeof(bool))
                {
                    double newValue = (bool)patientParameter.DynamicValue ? 1 : 0;
                    double oldValue = (bool)patientParameter.Value? 1 : 0;
                    effectiveness += (newValue - oldValue) * patientParameter.PositiveDynamicCoef;
                }
                else
                    effectiveness += 
                        (Convert.ToDouble(patientParameter.DynamicValue) - Convert.ToDouble(patientParameter.Value)) 
                        * patientParameter.PositiveDynamicCoef;
            }
            return new InfluenceResult()
            {
                InfluenceId = influenceId,
                InfluenceEffectiveness = effectiveness,
                TrackedParameters = influenceDynamicData.Parameters.Where(x => 
                x.Value.GetType() != typeof(string) && 
                x.DynamicValue.GetType() != typeof(string))
            };
        }

    }
}
