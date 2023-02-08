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
        public IDynamicAgentInitSettings GetSettingsBy(AgentType agentType, int patientId)
        {
            //TODO Хранить настройки в бд
            switch(agentType)
            {
                case AgentType.AgingPatient:
                    {
                        var sets = new DynamicAgentInitSettings()
                        {
                            Properties = new Dictionary<string, IProperty>
                            {
                                { "CurrentAge", new AgentProperty("CurrentAge", typeof(double)) },
                                { "CurrentBioAge", new AgentProperty("CurrentBioAge", typeof(double)) },
                                { "CurrentAgeRang", new AgentProperty("CurrentAgeRang", typeof(AgentBioAgeStates)) }
                            },
                            States = new Dictionary<string, IAgentState>(),
#warning А как тогда обновлять состояние агента, если нужен не Today? 08.02.2023
                            DetermineAgentPropertiesActions = 
                            $"parameters = GetLatestPatientParams({DateTime.MinValue},{DateTime.Today},{patientId})\n" +
                            $"age = GetAge(parameters)\n" +
                            $"bioAge = GetBioage(parameters)\n" +
                            $"rang = GetAgeRangBy(age,bioage)\n" +
                            $"CurrentAge = age\n" +
                            $"CurrentBioAge = bioAge\n" +
                            $"CurrentAgeRang = rang"
                        };
                           
                        foreach (AgentBioAgeStates state in AgentBioAgeStates.GetValues(typeof(AgentBioAgeStates)))
                        {
                            string stateName = state.GetDisplayAttributeValue();
                            sets.States[stateName] = new AgentState(stateName);
                        }
                        return sets;
                    }
                default: throw new NotImplementedException();
            }
        }
    }
}
