﻿using Agents.API.Data.Store;
using Agents.API.Entities.Events;
using Agents.API.Entities.Requests;
using Agents.API.Entities.Requests.Responce;
using ASMLib.DynamicAgent;
using ASMLib.Entities;
using ASMLib.EventBus;
using ASMLib.Requests;
using System.Collections.Concurrent;

namespace Agents.API.Service.Services
{
    public class AgentsService
    {
        private readonly IAgentsStore _agentsStore;
        private readonly IEventBus _eventBus;

        private ConcurrentDictionary<AgentKey, ConcurrentQueue<PredictionRequest>> _predictionsQueueDict;
        private ConcurrentDictionary<AgentKey, PredictionRequest> _currentPredictions;

        public AgentsService(IAgentsStore agentsStore, IEventBus eventBus)
        {
            _agentsStore = agentsStore;
            _eventBus = eventBus;
            _currentPredictions = new();
            _predictionsQueueDict = new();
        }


        public void AddPredictionRequest(AgentKey key, PredictionRequest request)
        {
            if (!_predictionsQueueDict.ContainsKey(key))
                _predictionsQueueDict[key] = new();

            _predictionsQueueDict[key].Enqueue(request);
        }


        public async Task<IEnumerable<Property>> GetAgentCurProperties(AgentKey Key)
        {
            IAgent agent = await _agentsStore.Get(Key);
            return agent.Properties.Values.Where(x => x.Description != null && x.Description != string.Empty);
        }


        public async Task<IEnumerable<Parameter>> GetAgentCalculationBuffer(AgentKey key)
        {
            IAgent agent = await _agentsStore.Get(key);
            return agent.Buffer.Values;
        }


        public async Task ProcessCurrentPredictions()
        {
            foreach (var pair in _predictionsQueueDict)
            {
                if (_currentPredictions.ContainsKey(pair.Key) 
                    || !pair.Value.Any() || !pair.Value.TryDequeue(out PredictionRequest request))
                    continue;
                _currentPredictions[pair.Key] = request;
                var responce = await GetPrediction(pair.Key, request);

                if (_currentPredictions.TryRemove(pair.Key, out _))
                    _eventBus.Publish(new PredictionResultEvent()
                    {
                        PredictionId = request.Id,
                        AgentKey = pair.Key,
                        StatePrediction = responce.StatePrediction,
                        ErrorMessage = responce.ErrorMessage
                    });
                else
                    pair.Value.Enqueue(request);
            }   
        }


        private async Task<StatePredictionResponce> GetPrediction(AgentKey key, PredictionRequest request)
        {
            GetAgentStateResponce stateResponce = await GetAgentState(
                        new GetAgentStateRequest(key, request.AgentSettings, request.Settings.Variables));

            if (stateResponce.IsError)
                return new StatePredictionResponce() { ErrorMessage = stateResponce.ErrorMessage };

            var properties = (await GetAgentCurProperties(key)).ToArray();
            var buffer = (await GetAgentCalculationBuffer(key)).ToArray();

            return new StatePredictionResponce()
            { StatePrediction = new StatePrediction(request.Settings.SettingsName, stateResponce.AgentState, properties, buffer) };
        }


        private async Task<GetAgentStateResponce> GetAgentState(GetAgentStateRequest request)
        {
            IAgent agent = await _agentsStore.Get(request.Key);
            agent.SetSettings(request.AgentsSettings);
            agent.UpdateVariables(request.Variables);
            UpdateStateResult result = await agent.UpdateState();
            return new GetAgentStateResponce()
            {
                AgentState = result.AgentState,
                ErrorMessage = result.ErrorMessage
            };
        }
    }
}
