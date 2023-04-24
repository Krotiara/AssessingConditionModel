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
        public GetAgentStateQuery(IAgentKey key, AgentsSettings agentsSettings, IEnumerable<Property> variables)
        {
            Key = key;
            AgentsSettings = agentsSettings;
            Variables = variables.Cast<IProperty>();
        }

        public IAgentKey Key { get; set; }

        public AgentsSettings AgentsSettings { get; set; }

        public IEnumerable<IProperty> Variables { get; }
    }

    public class GetAgentStateQueryHandler : IRequestHandler<GetAgentStateQuery, IAgentState>
    {

        private readonly IAgentsStore _agentsStore;
        private readonly ILogger<GetAgentStateQueryHandler> _logger;

        public GetAgentStateQueryHandler(IAgentsStore agentsStore, ILogger<GetAgentStateQueryHandler> logger)
        {
            _agentsStore = agentsStore;
            _logger = logger;
        }

        public async Task<IAgentState> Handle(GetAgentStateQuery request, CancellationToken cancellationToken)
        {
#warning Пока id пациента запит как int, но будет string  
            IAgent agent = _agentsStore.GetAgent(request.Key, request.AgentsSettings);
            agent.UpdateVariables(request.Variables);
            try
            {
                await agent.UpdateState();
            }
            catch(ExecuteCodeLineException ex)
            {
                throw new GetAgingStateException(
                    $"Ошибка обновления состояния агента {agent.Id}:{agent.Organization}: {ex.Message}");
            }
            catch(Exception ex)
            {
                throw new GetAgingStateException(
                    $"Непредвиденная ошибка обновления состояния агента {agent.Id}:{agent.Organization}: {ex.Message}");

            }
            return agent.CurrentState;
        }
    }
}
