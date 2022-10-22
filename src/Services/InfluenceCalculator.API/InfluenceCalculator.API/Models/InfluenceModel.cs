using Interfaces;
using System.Collections;
using System;
using System.Collections.Generic;

namespace InfluenceCalculator.API.Models
{
    public class InfluenceModel: IInfluenceEffectivenessCalculator
    {

        public IInfluenceResult CalculateInfluence(IInfluence<IPatient, IPatientParameter> patientData)
        {
            throw new NotImplementedException();
            //double effectiveness = 0;
            //foreach(IPatientParameter patientParameter in patientData.Parameters.Values)
            //{

            //    if (double.TryParse(patientParameter.Value, out _) && double.TryParse(patientParameter.DynamicValue, out _))
            //    {
            //        effectiveness +=
            //            (double.Parse(patientParameter.DynamicValue) - double.Parse(patientParameter.Value))
            //            * patientParameter.PositiveDynamicCoef;
            //    }
            //    else if (bool.TryParse(patientParameter.Value, out _) && bool.TryParse(patientParameter.DynamicValue, out _))
            //    {
            //        double newValue = bool.Parse(patientParameter.DynamicValue) ? 1 : 0;
            //        double oldValue = bool.Parse(patientParameter.Value) ? 1 : 0;
            //        effectiveness += (newValue - oldValue) * patientParameter.PositiveDynamicCoef;
            //    }
            //    else
            //        continue;
            //}
            //return new InfluenceResult()
            //{
            //    InfluenceId = patientData.InfluenceId,
            //    InfluenceEffectiveness = effectiveness,
            //    PatientDataId = patientData.Id
            //};
        }

    }
}
