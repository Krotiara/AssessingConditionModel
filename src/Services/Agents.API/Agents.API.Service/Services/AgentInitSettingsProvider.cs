using Agents.API.Entities.DynamicAgent;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class AgentInitSettingsProvider : IAgentInitSettingsProvider
    {

        private readonly IDynamicAgentInitSettings agingPatientSettings;

        public AgentInitSettingsProvider()
        {
            agingPatientSettings = InitAgingPatientSettings();
        }

        public IDynamicAgentInitSettings GetSettingsBy(AgentType agentType)
        {
            //TODO Хранить настройки в бд
            switch(agentType)
            {
                case AgentType.AgingPatient:
                     return agingPatientSettings;   
                default: throw new NotImplementedException();
            }
        }


        private IDynamicAgentInitSettings InitAgingPatientSettings()
        {
            Dictionary<string,IAgentState> states = new();
            foreach (AgentBioAgeStates state in AgentBioAgeStates.GetValues(typeof(AgentBioAgeStates)))
                states[state.GetDisplayAttributeValue()] = new AgentState(state.GetDisplayAttributeValue());

#warning Нужнен каст к PredictRequest predictRequest или объединение микросервисов.
            var sets = new DynamicAgentInitSettings(
                            $"parameters = " +
                            $"{SystemCommands.GetLatestPatientParameters}({CommonArgs.StartDateTime}, {CommonArgs.EndDateTime}, {CommonArgs.ObservedId})\n" +
                            $"age = {SystemCommands.GetAge}(parameters)\n" +
                            $"bioAge = {SystemCommands.GetBioageByFunctionalParameters}(parameters)\n" + 
                            $"rang = {SystemCommands.GetAgeRangBy}(age, bioAge)\n" +
                            $"CurrentAge = age\n" +
                            $"CurrentBioAge = bioAge\n" +
                            $"CurrentAgeRang = rang", AgentType.AgingPatient)
            {
                ActionsArgsReplaceDict = new Dictionary<CommonArgs, object>
                            {
                                { CommonArgs.StartDateTime, null },
                                { CommonArgs.EndDateTime, null },
                                { CommonArgs.ObservedId, null }
                            },
                Properties = new Dictionary<string, IProperty>
                            {
                                { "CurrentAge", new AgentProperty("CurrentAge", typeof(long)) },
                                { "CurrentBioAge", new AgentProperty("CurrentBioAge", typeof(long)) },
                                { "CurrentAgeRang", new AgentProperty("CurrentAgeRang", typeof(AgentBioAgeStates)) }
                            },
                StateDiagram = new StateDiagram(states, async x =>
                {     
                    //TODO Убрать из провайдера команду расчета ранга, поместить сюда, так как там как раз правило по изменению состояний агентов. 
                    AgentBioAgeStates rang = Enum.Parse<AgentBioAgeStates>(x.Properties["CurrentAgeRang"].Value.ToString());
                    IAgentState state = states[rang.GetDisplayAttributeValue()];
                    state.Timestamp = x.Timestamp;
                    state.NumericCharacteristic = (long)x.Properties["CurrentBioAge"].Value - (long)x.Properties["CurrentAge"].Value;
                    return state;                    
                })
            };
            return sets;
        }
    }
}
