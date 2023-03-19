using Agents.API.Data.Repository;
using Agents.API.Entities.DynamicAgent;
using Interfaces;
using Interfaces.DynamicAgent;
using MediatR;
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

        public GetAgentStateQueryHandler(IDynamicAgentsRepository dynamicAgentsRepository)
        {
            _dynamicAgentsRepository = dynamicAgentsRepository;
        }

        public async Task<IAgentState> Handle(GetAgentStateQuery request, CancellationToken cancellationToken)
        {
#warning Пока id пациента запит как int, но будет string
            IDynamicAgent agent = _dynamicAgentsRepository.GetAgent(request.AgentKey);
            if (agent == null) return null;
            agent.Settings.ActionsArgsReplaceDict[CommonArgs.EndDateTime] = request.Timestamp;
            agent.Settings.ActionsArgsReplaceDict[CommonArgs.StartDateTime] = DateTime.MinValue;
            await agent.UpdateState();
            return agent.Settings.StateDiagram.CurrentState;
        }
    }
}
