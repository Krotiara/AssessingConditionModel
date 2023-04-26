using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.AgentsSettings
{
    public class Agent : IAgent
    {
        private readonly ICodeExecutor _codeExecutor;

        private readonly string _stateResolveCode;

        public string Id { get; set; }

        public string Organization { get;}

        public AgentType AgentType { get;} 

        public IAgentState CurrentState { get; set; }

        public ConcurrentDictionary<string, IProperty> Properties { get;}

        public ConcurrentDictionary<string, IProperty> Variables { get;}

        public ConcurrentDictionary<string, IAgentState> States { get;}

        private readonly ConcurrentDictionary<string, IProperty> _commonProperties;

        public Agent(IAgentKey key, AgentsSettings settings, ICodeExecutor codeExecutor)
        {
            _codeExecutor = codeExecutor;
            _stateResolveCode = settings.StateResolveCode;
            Id = key.ObservedId;
            Organization = key.ObservedObjectAffilation;
            AgentType = settings.AgentType;
            Properties = new();
            Variables = new();
            States = new();
            _commonProperties = new();
            InitDicts(settings);
            InitCommonProperties(key);
        }


        public async Task UpdateState()
        {
            try
            {
                ConcurrentDictionary<string, IProperty> calculatedArgs = await _codeExecutor.ExecuteCode(_stateResolveCode, Variables, _commonProperties);
                string state = await UpdateStateBy(calculatedArgs);
                CurrentState = States[state];
                //TODO - set numeric characteristic. - сделать через указываемый через фронт параметр.
                foreach (var pair in calculatedArgs)
                    if (Properties.ContainsKey(pair.Key))
                        Properties[pair.Key] = pair.Value;
            }
            catch(DetermineStateException ex)
            {
                //TODO log
            }
        }


        public void UpdateVariables(IEnumerable<IProperty> vars)
        {
            foreach (IProperty p in vars)
                Variables[p.Name] = p;
        }


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


        private void InitDicts(AgentsSettings settings)
        {
            foreach (IProperty p in settings.StateProperties)
                Properties[p.Name] = p;
            foreach (IProperty p in settings.Variables)
                Variables[p.Name] = p;
            foreach (IAgentState s in settings.States)
                States[s.Name] = s;
        }


        private void InitCommonProperties(IAgentKey key)
        {
            _commonProperties[CommonProperties.Id.ToString()] = 
                new Property() { Name = CommonProperties.Id.ToString(), Type = typeof(string), Value = key.ObservedId };
            _commonProperties[CommonProperties.Organization.ToString()] =
                new Property() { Name = CommonProperties.Organization.ToString(), Type = typeof(string), Value = key.ObservedObjectAffilation};

        }


        private async Task<string> UpdateStateBy(ConcurrentDictionary<string, IProperty> calcArgs)
        {
            string stateVar = "isState";
            foreach(IAgentState state in States.Values)
            {
                string ifCondition = $"{stateVar}={state.DefinitionCode}";
                var args = await _codeExecutor.ExecuteCode(ifCondition, calcArgs, _commonProperties);
                if ((bool)args[stateVar].Value)
                    return state.Name;
            }
            throw new DetermineStateException($"Cannot define state by states conditions");
        }
    }
}
