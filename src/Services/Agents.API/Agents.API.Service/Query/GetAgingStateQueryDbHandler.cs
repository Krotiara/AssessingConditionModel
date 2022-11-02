using Agents.API.Data.Repository;
using Agents.API.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAgingStateQueryDbHandler : IRequestHandler<GetAgingStateQueryDb, AgingState>
    {
        private readonly IAgingStatesRepository agingStatesRepository;

        public GetAgingStateQueryDbHandler(IAgentPatientsRepository agentPatientsRepository)
        {
            this.agingStatesRepository = agingStatesRepository;
        }

        public async Task<AgingState> Handle(GetAgingStateQueryDb request, CancellationToken cancellationToken)
        {
            return await agingStatesRepository.GetStateAsync(request.PatientId, request.Timestamp);
        }
    }
}
