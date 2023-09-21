﻿using Agents.API.Data.Store;
using Agents.API.Entities;
using Agents.API.Entities.Documents;
using Agents.API.Entities.Requests;
using Agents.API.Interfaces;
using Amazon.Runtime.Internal.Util;
using Interfaces;
using Interfaces.DynamicAgent;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class AgentsService
    {
        private readonly IAgentsStore _agentsStore;
        private readonly ILogger<AgentsService> _logger;

        public AgentsService(IAgentsStore agentsStore, ILogger<AgentsService> logger)
        {
            _agentsStore = agentsStore;
            _logger = logger;
        }


        public IAgent GetAgent(IAgentKey key, AgentSettings settings) => _agentsStore.GetAgent(key, settings);


        public async Task<IAgentState?> GetAgentState(GetAgentStateRequest request)
        {
            IAgent agent = _agentsStore.GetAgent(request.Key, request.AgentsSettings);
            agent.UpdateVariables(request.Variables);
            try
            {
                await agent.UpdateState();
            }
            catch (ExecuteCodeLineException ex)
            {
                _logger.LogError($"Ошибка обновления состояния агента {agent.Id}:{agent.Affiliation}: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Непредвиденная ошибка обновления состояния агента {agent.Id}:{agent.Affiliation}: {ex.Message}");
                return null;

            }
            return agent.CurrentState;
        }
    }
}
