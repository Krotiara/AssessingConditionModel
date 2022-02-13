using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.Agents
{
    public class AgentPatient : Agent
    {
        private readonly Patient patient;

        public AgentPatient(Patient patient)
        {
            this.patient = patient;
        }


        public override void InitStateDiagram()
        {
            StateDiagram = new StateDiagram();
            State criticalState = StateDiagram.AddState("Critical");
            State sickState = StateDiagram.AddState("Sick");
            State healthyState = StateDiagram.AddState("Healthy");
            StateDiagram.DetermineState = DetermineState;
        }


        private State DetermineState()
        {
            double temp = patient.ClinicalParams.GetParam<double>(ClinicalParams.Temperature);
            double critLowTemp = patient.ParamsNorms.GetParam<double>(NormParams.LowCriticalTemperature);  
            double critUpTemp = patient.ParamsNorms.GetParam<double>(NormParams.UpCriticalTemperature);
            double normLowTemp = patient.ParamsNorms.GetParam<double>(NormParams.LowNormalTemperature);
            double normUpTemp = patient.ParamsNorms.GetParam<double>(NormParams.UpNormalTemperature);

            double saturation = patient.ClinicalParams.GetParam<double>(ClinicalParams.Saturation);
            double normLowSaturation = patient.ParamsNorms.GetParam<double>(NormParams.LowNormalSaturation);
            double critLowSaturation = patient.ParamsNorms.GetParam<double>(NormParams.LowCriticalSaturation);

            bool isCough = patient.ClinicalParams.GetParam<bool>(ClinicalParams.Cough);

            double lungDamage = patient.ClinicalParams.GetParam<double>(ClinicalParams.LungTissueDamage);
            double critLungDamage = patient.ParamsNorms.GetParam<double>(NormParams.UpCriticalLungTissueDamage);

            double cProtein = patient.ClinicalParams.GetParam<double>(ClinicalParams.CReactiveProtein);
            double critCProtein = patient.ParamsNorms.GetParam<double>(NormParams.UpCriticalCReactiveProtein);
            double normCProtein = patient.ParamsNorms.GetParam<double>(NormParams.UpNormCReactiveProtein);

            if (normLowTemp <= temp && temp <= normUpTemp &&
            normLowSaturation <= saturation && !isCough &&
            lungDamage == 0 && cProtein <= normCProtein)
                return StateDiagram.GetState("Healthy");

            if ((critLowTemp < temp && temp < normLowTemp) ||
            (normUpTemp < temp && temp < critUpTemp) ||
            (critLowSaturation < saturation && saturation < normLowSaturation) ||
            isCough ||
            (lungDamage > 0 && lungDamage < critLungDamage) ||
            (normCProtein < cProtein && cProtein < critCProtein))
                return StateDiagram.GetState("Sick");

            if (temp <= critLowTemp ||
            temp >= critUpTemp ||
            saturation <= critLowSaturation ||
            lungDamage >= critLungDamage ||
            cProtein >= critCProtein)
                return StateDiagram.GetState("Critical");

            throw new StateDetermineException($"Cant determine state for Patient with name {patient.Name}");
        }
    }
}
