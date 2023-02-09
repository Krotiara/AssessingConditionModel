using Agents.API.Entities.DynamicAgent;
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
        public IDynamicAgentInitSettings GetSettingsBy(AgentType agentType)
        {
            //TODO Хранить настройки в бд
            switch(agentType)
            {
                case AgentType.AgingPatient:
                     return GetAgingPatientSettings();   
                default: throw new NotImplementedException();
            }
        }


        private IDynamicAgentInitSettings GetAgingPatientSettings()
        {
            Dictionary<string,IAgentState> states = new();
            foreach (AgentBioAgeStates state in AgentBioAgeStates.GetValues(typeof(AgentBioAgeStates)))
                states[state.GetDisplayAttributeValue()] = new AgentState(state.GetDisplayAttributeValue());

                var sets = new DynamicAgentInitSettings(
                            $"parameters = GetLatestPatientParams({CommonArgs.StartDateTime}, {CommonArgs.EndDateTime}, {CommonArgs.ObservedId})\n" +
                            $"age = GetAge(parameters)\n" +
                            $"bioAge = GetBioage(parameters)\n" +
                            $"rang = GetAgeRangBy(age, bioage)\n" +
                            $"CurrentAge = age\n" +
                            $"CurrentBioAge = bioAge\n" +
                            $"CurrentAgeRang = rang")
            {
                ActionsArgsReplaceDict = new Dictionary<CommonArgs, object>
                            {
                                { CommonArgs.StartDateTime, null },
                                { CommonArgs.EndDateTime, null },
                                { CommonArgs.ObservedId, null }
                            },
                Properties = new Dictionary<string, IProperty>
                            {
                                { "CurrentAge", new AgentProperty("CurrentAge", typeof(double)) },
                                { "CurrentBioAge", new AgentProperty("CurrentBioAge", typeof(double)) },
                                { "CurrentAgeRang", new AgentProperty("CurrentAgeRang", typeof(AgentBioAgeStates)) }
                            },
                StateDiagram = new StateDiagram(states, async x =>
                {
                    AgentBioAgeStates rang = (AgentBioAgeStates)x.Properties["CurrentAgeRang"].Value;
                    return states[rang.GetDisplayAttributeValue()];
                })
            };

            


            return sets;
        }
    }
}
