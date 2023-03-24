using Agents.API.Entities.DynamicAgent;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class AgentInitSettingsProvider : IAgentInitSettingsProvider
    {
        private readonly ConcurrentDictionary<AgentType, IDynamicAgentInitSettings> _initSettings;
        private readonly ConcurrentDictionary<string, IAgentState> _agingPatientStates;
        private readonly ConcurrentDictionary<string, IAgentState> _dentistPatientStates;

        public AgentInitSettingsProvider()
        {
            _agingPatientStates = new();
            foreach (AgentBioAgeStates state in AgentBioAgeStates.GetValues(typeof(AgentBioAgeStates)))
                _agingPatientStates[state.GetDisplayAttributeValue()] = new AgentState(state.GetDisplayAttributeValue());

            _dentistPatientStates = new();
            foreach (DentistAgentStates state in DentistAgentStates.GetValues(typeof(DentistAgentStates)))
                _dentistPatientStates[state.GetDisplayAttributeValue()] = new AgentState(state.GetDisplayAttributeValue());

            _initSettings = new ConcurrentDictionary<AgentType, IDynamicAgentInitSettings>();
            _initSettings[AgentType.AgingPatient] = InitAgingPatientSettings();
            _initSettings[AgentType.DentistPatient] = InitDentistPatientAgentSettings();
        }

        public IDynamicAgentInitSettings GetSettingsBy(AgentType agentType)
        {
            return _initSettings.ContainsKey(agentType) ? _initSettings[agentType] : null;
        }


        private IDynamicAgentInitSettings InitAgingPatientSettings()
        {
            var sets = new DynamicAgentInitSettings(
                            $"parameters = " +
                            $"{SystemCommands.GetLatestPatientParameters}({CommonArgs.StartDateTime}, " +
                            $"{CommonArgs.EndDateTime}, {CommonArgs.ObservedId}, {CommonArgs.MedicalOrganization})\n" +
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
                                { "CurrentAge", new AgentProperty("CurrentAge", typeof(int)) },
                                { "CurrentBioAge", new AgentProperty("CurrentBioAge", typeof(int)) },
                                { "CurrentAgeRang", new AgentProperty("CurrentAgeRang", typeof(AgentBioAgeStates)) }
                            },
                StateDiagram = new StateDiagram(_agingPatientStates, async x =>
                {     
                    //TODO Убрать из провайдера команду расчета ранга, поместить сюда, так как там как раз правило по изменению состояний агентов. 
                    AgentBioAgeStates rang = Enum.Parse<AgentBioAgeStates>(x.Properties["CurrentAgeRang"].Value.ToString());
                    IAgentState state = _agingPatientStates[rang.GetDisplayAttributeValue()];
                    state.Timestamp = x.Timestamp;
                    state.NumericCharacteristic = (int)x.Properties["CurrentBioAge"].Value - (int)x.Properties["CurrentAge"].Value;
                    return state;                    
                })
            };
            return sets;
        }
    

        private IDynamicAgentInitSettings InitDentistPatientAgentSettings()
        {
            return new DynamicAgentInitSettings(
                            $"parameters = " +
                            $"{SystemCommands.GetLatestPatientParameters}({CommonArgs.StartDateTime}, " +
                            $"{CommonArgs.EndDateTime}, {CommonArgs.ObservedId}, {CommonArgs.MedicalOrganization})\n" +
                            $"sum = {SystemCommands.GetDentistSum}(parameters)\n" +
                            $"CurrentSum = sum", AgentType.DentistPatient)
            {
                ActionsArgsReplaceDict = new Dictionary<CommonArgs, object>{
                               { CommonArgs.StartDateTime, null },
                               { CommonArgs.EndDateTime, null },
                               { CommonArgs.ObservedId, null }},
                Properties = new Dictionary<string, IProperty>
                            {
                                { "CurrentSum", new AgentProperty("CurrentSum", typeof(double)) }
                            },
                StateDiagram = new StateDiagram(_dentistPatientStates, async x =>
                {
                    //TODO Убрать из провайдера команду расчета ранга, поместить сюда, так как там как раз правило по изменению состояний агентов. 
                    float sum = (float)x.Properties["CurrentSum"].Value;
                    DentistAgentStates state;
                    if (sum <= 6)
                        state = DentistAgentStates.RangI;
                    else if (sum >= 7 && sum <= 13)
                        state = DentistAgentStates.RangII;
                    else if (sum > 14 && sum <= 20)
                        state = DentistAgentStates.RangIII;
                    else
                        state = DentistAgentStates.RangIV;

                    IAgentState agentState = _dentistPatientStates[state.GetDisplayAttributeValue()];
                    agentState.NumericCharacteristic = sum;
                    return agentState;
                })
            };
        }
    }

    
    enum DentistAgentStates
    {
        [Display(Name = "Легкая степень тяжести")]
        RangI = 1,
        [Display(Name = "Средняя степень тяжести")]
        RangII = 2,
        [Display(Name = "Высокая степень тяжести")]
        RangIII = 3,
        [Display(Name = "Очень высокая степень тяжести")]
        RangIV = 4,
    }

}
