using Agents.API.Data.Repository;
using Agents.API.Entities;
using Agents.API.Entities.AgentsSettings;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{

    public class GetAgentStateQuery: IRequest<IAgentState>
    {
        public GetAgentStateQuery(AgentKey key, DateTime timestamp)
        {
            AgentKey = key;
            Timestamp = timestamp;
        }

        public AgentKey AgentKey { get; }

        public DateTime Timestamp { get; }
    }

    public class GetAgentStateQueryHandler : IRequestHandler<GetAgentStateQuery, IAgentState>
    {

        private readonly IDynamicAgentsRepository _dynamicAgentsRepository;
        private readonly ILogger<GetAgentStateQueryHandler> _logger;

        public GetAgentStateQueryHandler(IDynamicAgentsRepository dynamicAgentsRepository, ILogger<GetAgentStateQueryHandler> logger)
        {
            _dynamicAgentsRepository = dynamicAgentsRepository;
            _logger = logger;
        }

        public async Task<IAgentState> Handle(GetAgentStateQuery request, CancellationToken cancellationToken)
        {
#warning Пока id пациента запит как int, но будет string
            
                IAgent agent = _dynamicAgentsRepository.GetAgent(request.AgentKey);
                if (agent == null) return null;
                agent.Settings.ActionsArgsReplaceDict[CommonArgs.EndDateTime] = request.Timestamp;
                agent.Settings.ActionsArgsReplaceDict[CommonArgs.StartDateTime] = DateTime.MinValue;
            try
            {
                await agent.UpdateState();
            }
            catch(ExecuteCodeLineException ex)
            {
                throw new GetAgingStateException($"Ошибка обновления состояния агента {agent.ObservedId}:{agent.ObservedObjectAffilation}: {ex.Message}");
            }
            catch(Exception ex)
            {
                throw new GetAgingStateException($"Непредвиденная ошибка обновления состояния агента {agent.ObservedId}:{agent.ObservedObjectAffilation}: {ex.Message}");

            }
            return agent.Settings.StateDiagram.CurrentState;
        }
    }
}
