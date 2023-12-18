﻿using Agents.API.Entities.Documents;
using Agents.API.Interfaces;
using ASMLib.DynamicAgent;
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

        public string Affiliation { get; }

        public string AgentType { get; }

        public IAgentState CurrentState { get; set; }

        public ConcurrentDictionary<string, IProperty> Properties { get; }

        public ConcurrentDictionary<string, IProperty> Variables { get; }

        public ConcurrentDictionary<(string, DateTime), IParameter> Buffer { get; set; }

        public ConcurrentDictionary<string, IAgentState> States { get; }

        private readonly AgentPropertiesNamesSettings _commonPropertiesNames;

        public Agent(IAgentKey key, AgentSettings settings, ICodeExecutor codeExecutor)
        {
            _codeExecutor = codeExecutor;
            _stateResolveCode = settings.StateResolveCode;
            Id = key.ObservedId;
            Affiliation = key.ObservedObjectAffilation;
            AgentType = settings.AgentType;
            Properties = new();
            Variables = new();
            States = new();
            Buffer = new();
            InitDicts(settings);
            InitCommonProperties(key, settings.CommonNamesSettings);
            _commonPropertiesNames = settings.CommonNamesSettings; //TODO инициализация нового экземпляра, чтобы не продлевать время жизни settings
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
                        return new UpdateStateResult() { Status = UpdateStateStatus.Error, ErrorMessage = res.ErrorMessage };
                    if ((bool)Variables[stateVar].Value)
                    {
                        //TODO убрать обращение к Variables
                        //TODO отделить состояние от агента.
                        state.NumericCharacteristic = Convert.ToDouble(Variables[_commonPropertiesNames.StateNumber].Value);
                        state.Timestamp = Properties[_commonPropertiesNames.EndTimestamp].ConvertValue<DateTime>();
                        CurrentState = States[state.Name];
                        return new UpdateStateResult() { Status = UpdateStateStatus.Success };
                    }
                }
            }

            return new UpdateStateResult() { Status = UpdateStateStatus.Error, ErrorMessage = res.ErrorMessage };
            //TODO - set numeric characteristic. - сделать через указываемый через фронт параметр
        }


        public void UpdateVariables(IEnumerable<IProperty> vars)
        {
            foreach (IProperty p in vars)
                Variables[p.Name] = p;
            Buffer.Clear();
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


        private void InitDicts(AgentSettings settings)
        {
            foreach (IProperty p in settings.StateProperties)
                Properties[p.Name] = p;
            foreach (IProperty p in settings.Variables)
                Variables[p.Name] = p;
            foreach (IAgentState s in settings.States)
                States[s.Name] = s;
        }


        private void InitCommonProperties(IAgentKey key, AgentPropertiesNamesSettings settings)
        {
            Properties[settings.Id] =
                new Property(settings.Id, typeof(string).FullName, key.ObservedId);
            Properties[settings.Affiliation] =
                new Property(settings.Affiliation, typeof(string).FullName, key.ObservedObjectAffilation);
        }
    }
}
