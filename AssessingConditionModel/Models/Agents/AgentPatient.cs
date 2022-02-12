using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models.Agents
{
    public class AgentPatient : Agent
    {
        private Patient patient;

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
            double temp = patient.GetParamValue<double>(ParamsNames.Temperature);
            double critLowTemp = patient.GetParamNormValue<double>(ParamsNames.LowCriticalTemperature);
            double critUpTemp = patient.GetParamNormValue<double>(ParamsNames.UpCriticalTemperature);
            double normLowTemp = patient.GetParamNormValue<double>(ParamsNames.LowNormalTemperature);
            double normUpTemp = patient.GetParamNormValue<double>(ParamsNames.UpNormalTemperature);

            double saturation = patient.GetParamValue<double>(ParamsNames.Saturation);
            double normLowSaturation = patient.GetParamNormValue<double>(ParamsNames.LowNormalSaturation);
            double critLowSaturation = patient.GetParamNormValue<double>(ParamsNames.LowCriticalSaturation);

            bool isCough = patient.GetParamValue<bool>(ParamsNames.Cough);

            double lungDamage = patient.GetParamValue<double>(ParamsNames.LungTissueDamage);
            double critLungDamage = patient.GetParamNormValue<double>(ParamsNames.UpCriticalLungTissueDamage);

            double cProtein = patient.GetParamValue<double>(ParamsNames.CReactiveProtein);
            double critCProtein = patient.GetParamNormValue<double>(ParamsNames.UpCriticalCReactiveProtein);
            double normCProtein = patient.GetParamNormValue<double>(ParamsNames.UpNormCReactiveProtein);

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
