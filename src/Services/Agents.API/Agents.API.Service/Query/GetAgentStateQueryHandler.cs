using Agents.API.Data.Repository;
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
        public GetAgentStateQuery(string agentId, AgentType agentType, DateTime timestamp)
        {
            AgentId = agentId;
            AgentType = agentType;
            Timestamp = timestamp;
        }

        public string AgentId { get; }

        public AgentType AgentType { get; }

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
            int id = int.Parse(request.AgentId);
            IDynamicAgent agent = _dynamicAgentsRepository.GetAgent(id, request.AgentType);
            if (agent == null)
                return null;
            agent.Settings.ActionsArgsReplaceDict[CommonArgs.EndDateTime] = request.Timestamp;
            agent.Settings.ActionsArgsReplaceDict[CommonArgs.StartDateTime] = DateTime.MinValue;
            await agent.UpdateState();
            return agent.Settings.StateDiagram.CurrentState;
        }
    }
}
