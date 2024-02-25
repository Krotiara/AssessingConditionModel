using Agents.API.Entities.Documents;
using Agents.API.Interfaces;
using ASMLib.DynamicAgent;
using Interfaces;
using ASMLib.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMLib.Entities;

namespace Agents.API.Entities.AgentsSettings
{
    public class Agent : IAgent
    {
        private readonly ICodeExecutor _codeExecutor;

        private string _stateResolveCode;

        public string Id { get; set; }

        public string Affiliation { get; }

        public string AgentType { get; private set; }

        public IAgentState CurrentState { get; set; }

        public ConcurrentDictionary<string, Property> Properties { get; }

        public ConcurrentDictionary<string, Property> Variables { get; }

        public ConcurrentDictionary<(string, DateTime), Parameter> Buffer { get; set; }

        public ConcurrentDictionary<string, IAgentState> States { get; }

        private AgentPropertiesNamesSettings _commonPropertiesNames;

        public Agent(IAgentKey key, ICodeExecutor codeExecutor)
        {
            _codeExecutor = codeExecutor;
            Id = key.ObservedId;
            Affiliation = key.ObservedObjectAffilation;
            Properties = new();
            Variables = new();
            States = new();
            Buffer = new();
            
        }


        public async Task<UpdateStateResult> UpdateState()
        {
            ExecuteCodeResult res = await _codeExecutor.ExecuteCode(_stateResolveCode, this, _commonPropertiesNames);
            if (res.Status != ExecuteCodeStatus.Error)
            {
                string stateVar = "isState";

                foreach (IAgentState state in States.Values)
                {
                    string ifCondition = $"{stateVar}={state.DefinitionCode}";
                    res = await _codeExecutor.ExecuteCode(ifCondition, this, _commonPropertiesNames);
                    if (res.Status == ExecuteCodeStatus.Error)
                        return new UpdateStateResult() { ErrorMessage = res.ErrorMessage };
                    if ((bool)Variables[stateVar].Value)
                    {
                        //TODO убрать обращение к Variables
                        //TODO отделить состояние от агента.
                        state.NumericCharacteristic = Convert.ToDouble(Properties[_commonPropertiesNames.StateNumber].Value);
                        state.Timestamp = Variables[_commonPropertiesNames.EndTimestamp].ConvertValue<DateTime>();
                        CurrentState = States[state.Name];
                        return new UpdateStateResult() { AgentState = CurrentState };
                    }
                }
            }

            return new UpdateStateResult() {ErrorMessage = res.ErrorMessage};
            //TODO - set numeric characteristic. - сделать через указываемый через фронт параметр
        }


        public void UpdateVariables(IEnumerable<Property> vars)
        {
            foreach (Property p in vars)
                Variables[p.Name] = p;
            Buffer.Clear();
        }


        public void AddToBuffer(Parameter parameter) => Buffer.TryAdd((parameter.Name, parameter.Timestamp), parameter);


        public T GetPropertyValue<T>(string propertyName)
        {
            if (!typeof(T).Equals(Properties[propertyName].Type) && !typeof(T).IsEnum)
                throw new GetAgentPropertyValueException($"Несоответсвие типов переданного типа и типа параметра");
            try
            {
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), Properties[propertyName].Value.ToString());
                }
                else
                    return (T)Properties[propertyName].Value;
            }
            catch (Exception ex)
            {
                throw new GetAgentPropertyValueException("Непредвиденная ошибка поулчения параметра агента", ex);
            }
        }


        private void InitDicts(IAgentSettings settings)
        {
            Properties.Clear();
            Variables.Clear();
            States.Clear();
            foreach (Property p in settings.StateProperties)
                Properties[p.Name] = p;
            foreach (Property p in settings.Variables)
                Variables[p.Name] = p;
            foreach (IAgentState s in settings.States)
                States[s.Name] = s;
        }


        private void InitCommonProperties(string observingId, string observingAffiliation, AgentPropertiesNamesSettings settings)
        {
            Properties[settings.Id] =
                new Property(settings.Id, typeof(string).FullName, observingId);
            Properties[settings.Affiliation] =
                new Property(settings.Affiliation, typeof(string).FullName, observingAffiliation);
        }

        public void SetSettings(IAgentSettings settings)
        {
            _stateResolveCode = settings.StateResolveCode;
            AgentType = settings.AgentType;
            InitDicts(settings);
            InitCommonProperties(Id, Affiliation, settings.CommonNamesSettings);
            _commonPropertiesNames = settings.CommonNamesSettings; //TODO инициализация нового экземпляра, чтобы не продлевать время жизни settings
        }
    }
}
