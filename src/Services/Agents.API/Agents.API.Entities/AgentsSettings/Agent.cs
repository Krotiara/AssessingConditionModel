using Agents.API.Interfaces;
using Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.AgentsSettings
{
    public class Agent
    {
        private readonly ICodeExecutor _codeExecutor;

        private readonly string _stateResolveCode;

        private readonly string _codeStateName = "State";

        public string Id { get; set; }

        public string Organization { get;}

        public AgentType AgentType { get;} 

        public AgentState CurrentState { get; set; }

        public ConcurrentDictionary<string, Property> Properties { get; private set; }

        public ConcurrentDictionary<string, Property> Variables { get; private set; }

        public ConcurrentDictionary<string, AgentState> States { get; private set; }

        public Agent(string id, string organization, AgentsSettings settings, ICodeExecutor codeExecutor)
        {
            _codeExecutor = codeExecutor;
            _stateResolveCode = settings.StateResolveCode;
            Id = id;
            Organization = organization;
            AgentType = settings.AgentType;
            InitDicts(settings);
        }


        public async Task UpdateState()
        {
            Dictionary<string, Property> calculatedArgs = await _codeExecutor.ExecuteCode(_stateResolveCode, Variables);
            foreach(var pair in calculatedArgs)
            {
                if (pair.Key.Equals(_codeStateName) && pair.Value.Value != null)
                {
                    string stateName = pair.Value.Value as string;
                    if (States.ContainsKey(stateName))
                        CurrentState = States[stateName];
                    else
                        return; //TODO log
                }
                else
                    return; //TODO log
                if (Properties.ContainsKey(pair.Key))
                    Properties[pair.Key] = pair.Value;
            }
        }


        public void UpdateVariables(List<Property> vars)
        {
            foreach (Property p in vars)
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
            Properties = new();
            Variables = new();
            States = new();
            foreach (Property p in settings.StateProperties)
                Properties[p.Name] = p;
            foreach (Property p in settings.Variables)
                Variables[p.Name] = p;
            foreach (AgentState s in settings.States)
                States[s.Name] = s;
        }
    }
}
