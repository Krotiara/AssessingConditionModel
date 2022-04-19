using AssessingConditionModel.Models.PatientModel;

namespace AssessingConditionModel.Models.Agents
{

    


    public class AgentBioAge : Agent
    {
       
        public double Age { get; private set; }

        public double BioAge => CalculateBioAge();

        public double AgeDelta => BioAge - Age;

        //TODO может заменить на словарь?
        public double SystolicBloodPressure { get; private set; }

        public double DiastolicBloodPressure { get; private set; }

        public double InhaleBreathHolding { get; private set; }

        public double OuthaleBreathHolding { get; private set; }

        public double LungCapacity { get; private set; }

        public double Weight { get; private set; }

        public double Accommodation { get; private set; }

        public double HearingAcuity { get; private set; }

        public double StaticBalancing { get; private set; }


        public AgentBioAge(string name)
        {
            Name = name;
            InitStateDiagram();
        }

        public override void InitStateDiagram()
        {
            StateDiagram = new StateDiagram();

            foreach(AgentBioAgeStates state in AgentBioAgeStates.GetValues(typeof(AgentBioAgeStates)))
            {
                StateDiagram.AddState(state.GetDisplayAttributeValue());
            }
            StateDiagram.DetermineState = DetermineState;
        }

        private State DetermineState()
        {
            //TODO check ненулевые значения.
            double ageDelta = AgeDelta;
            AgentBioAgeStates rang;
            if (ageDelta <= -9)
                rang = AgentBioAgeStates.RangI;
            else if (ageDelta > -9 && ageDelta <= -3)
                rang = AgentBioAgeStates.RangII;
            else if (ageDelta > -3 && ageDelta <= 3)
                rang = AgentBioAgeStates.RangIII;
            else if (ageDelta > 3 && ageDelta <= 9)
                rang = AgentBioAgeStates.RangIV;
            else
                rang = AgentBioAgeStates.RangV;

            return StateDiagram.GetState(rang.GetDisplayAttributeValue());
        }


        public override void ProcessMessage(Message message, Agent messenger)
        {
            throw new System.NotImplementedException();
        }


        public override void ProcessPrivateTransitions()
        {
            throw new System.NotImplementedException();
        }


        private double CalculateBioAge()
        {
            return -1.07 * SystolicBloodPressure
                + 1.1 * DiastolicBloodPressure
                + 1.94 * (SystolicBloodPressure - DiastolicBloodPressure)
                - 1.45 * InhaleBreathHolding
                + 1.32 * OuthaleBreathHolding
                - 3.46 * LungCapacity
                + 0.15 * Weight
                - 4.35 * Accommodation
                + 5.57 * HearingAcuity
                - 2.6 * StaticBalancing;
        }
    }
}
