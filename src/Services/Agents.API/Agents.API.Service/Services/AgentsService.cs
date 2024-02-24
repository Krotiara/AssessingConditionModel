using Agents.API.Data.Store;
using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Documents;
using Agents.API.Entities.Requests;
using Agents.API.Entities.Requests.Responce;
using ASMLib.DynamicAgent;
using Interfaces;
using Interfaces.DynamicAgent;
using Microsoft.Extensions.Logging;

namespace Agents.API.Service.Services
{
    public class AgentsService
    {
        private readonly IAgentsStore _agentsStore;

        public AgentsService(IAgentsStore agentsStore)
        {
            _agentsStore = agentsStore;
        }


        public Task<IAgent> GetAgent(IAgentKey key, AgentSettings settings) => _agentsStore.GetAgent(key, settings);


        public async Task<GetAgentStateResponce> GetAgentState(GetAgentStateRequest request)
        {
            IAgent agent = await _agentsStore.GetAgent(request.Key, request.AgentsSettings);
            agent.UpdateVariables(request.Variables);
            UpdateStateResult result = await agent.UpdateState();
            return new GetAgentStateResponce()
            {
                AgentState = result.AgentState,
                ErrorMessage = result.ErrorMessage
            };
        }


        public async Task<IEnumerable<IProperty>> GetAgentCurProperties(IAgentKey Key, AgentSettings agentsSettings)
        {
            IAgent agent = await _agentsStore.GetAgent(Key, agentsSettings);
            return agent.Properties.Values.Where(x => x.Description != null && x.Description != string.Empty);
        }


        public async Task<IEnumerable<IProperty>> GetAgentCurVariables(IAgentKey Key, AgentSettings agentsSettings)
        {
            IAgent agent = await _agentsStore.GetAgent(Key, agentsSettings);
            return agent.Variables.Values.Where(x => x.Description != null && x.Description != string.Empty);
        }


        public async Task<IEnumerable<IParameter>> GetAgentCalculationBuffer(IAgentKey key, AgentSettings agentsSettings)
        {
            IAgent agent = await _agentsStore.GetAgent(key, agentsSettings);
            return agent.Buffer.Values;
        }


        public async Task<StatePredictionResponce> GetPrediction(IAgentKey key,
            AgentSettings sets, PredictionSettings pSets)
        {
            GetAgentStateResponce stateResponce = await GetAgentState(
                        new GetAgentStateRequest(key, sets, pSets.Variables));

            if (stateResponce.IsError)
                return new StatePredictionResponce() { ErrorMessage = stateResponce.ErrorMessage };

            var properties = await GetAgentCurProperties(key, sets);
            var buffer = await GetAgentCalculationBuffer(key, sets);

            return new StatePredictionResponce()
            { StatePrediction = new StatePrediction(pSets.SettingsName, stateResponce.AgentState, properties, buffer) };
        }
    }
}
