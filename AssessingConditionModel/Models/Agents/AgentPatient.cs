using AssessingConditionModel.Models.PatientModel;
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
            Name = patient.MedicalHistoryNumber.ToString();
            InitStateDiagram();
        }


        public override void InitStateDiagram()
        {
            StateDiagram = new StateDiagram();
            State criticalState = StateDiagram.AddState("Critical");
            State sickState = StateDiagram.AddState("Sick");
            State healthyState = StateDiagram.AddState("Healthy");
            StateDiagram.DetermineState = DetermineState;
        }

        public override void ProcessMessage(Message message, Agent messenger)
        {
            throw new NotImplementedException();
        }

        public override void ProcessPrivateTransitions()
        {
            throw new NotImplementedException();
        }

        
        private State DetermineState()
        {

            if (patient.ParametersNorms.LowNormalTemperature <= patient.ClinicalParameters.Temperature
                && patient.ClinicalParameters.Temperature <= patient.ParametersNorms.UpNormalTemperature
                && patient.ParametersNorms.LowNormalSaturation <= patient.ClinicalParameters.Saturation
                && !patient.ClinicalParameters.IsCough
                && patient.ClinicalParameters.LungsModel.DamagePercent == 0
                && patient.ClinicalParameters.CReactiveProtein <= patient.ParametersNorms.UpNormCReactiveProtein)
                return StateDiagram.GetState("Healthy");

            if ((patient.ParametersNorms.LowCriticalTemperature < patient.ClinicalParameters.Temperature
                && patient.ClinicalParameters.Temperature < patient.ParametersNorms.LowNormalTemperature) ||
                (patient.ParametersNorms.UpNormalTemperature < patient.ClinicalParameters.Temperature
                && patient.ClinicalParameters.Temperature < patient.ParametersNorms.UpCriticalTemperature) ||
                (patient.ParametersNorms.LowCriticalSaturation < patient.ClinicalParameters.Saturation
                && patient.ClinicalParameters.Saturation < patient.ParametersNorms.LowNormalSaturation) ||
                patient.ClinicalParameters.IsCough ||
                (patient.ClinicalParameters.LungsModel.DamagePercent > 0
                && patient.ClinicalParameters.LungsModel.DamagePercent < patient.ParametersNorms.UpCriticalLungDamage) ||
                (patient.ParametersNorms.UpNormCReactiveProtein < patient.ClinicalParameters.CReactiveProtein
                && patient.ClinicalParameters.CReactiveProtein < patient.ParametersNorms.UpCriticalCReactiveProtein))
                return StateDiagram.GetState("Sick");

            if (patient.ClinicalParameters.Temperature <= patient.ParametersNorms.LowCriticalTemperature ||
                patient.ClinicalParameters.Temperature >= patient.ParametersNorms.UpCriticalTemperature ||
                patient.ClinicalParameters.Saturation <= patient.ParametersNorms.LowCriticalSaturation ||
                patient.ClinicalParameters.LungsModel.DamagePercent >= patient.ParametersNorms.UpCriticalLungDamage ||
                patient.ClinicalParameters.CReactiveProtein >= patient.ParametersNorms.UpCriticalCReactiveProtein)
                return StateDiagram.GetState("Critical");

            throw new StateDetermineException($"Cant determine state for Patient with name {patient.Name}");
        }
    }
}
