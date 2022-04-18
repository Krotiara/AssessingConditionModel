using AssessingConditionModel.Models.PatientModel;

namespace AssessingConditionModel.Models.Agents
{
    public class AgentBioHealth : Agent
    {

        public FunctionalParameters FunctionalParameters { get; set; }

        public AgentBioHealth(FunctionalParameters functionalParameters)
        {
            FunctionalParameters = functionalParameters;
        }

        public override void InitStateDiagram()
        {
            throw new System.NotImplementedException();
        }


        private double CalculateBioHealth()
        {
            return -1.07 * FunctionalParameters.SystolicBloodPressure 
                + 1.1 * FunctionalParameters.DiastolicBloodPressure
                +1.94 * FunctionalParameters.DeltaBloodPressure
                +

        }
    }
}
