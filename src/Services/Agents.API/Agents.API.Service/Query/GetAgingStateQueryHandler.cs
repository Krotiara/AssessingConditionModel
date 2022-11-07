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
    public class GetAgingStateQueryHandler : IRequestHandler<GetAgingStateQuery, AgingState>
    {
        private readonly IAgentPatientsRepository agentPatientsRepository;

        public GetAgingStateQueryHandler(IAgentPatientsRepository agentPatientsRepository)
        {
            this.agentPatientsRepository = agentPatientsRepository;
        }

        public async Task<AgingState> Handle(GetAgingStateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                AgentPatient agentPatient = await agentPatientsRepository.GetAgentPatient(request.PatientId);
                if (agentPatient == null)
                    throw new GetAgingStateException($"Agent patient for patient with id = {request.PatientId} not found.");
                AgingState state = new AgingState()
                {
                    PatientId = request.PatientId,
                    Age = agentPatient.CurrentAge,
                    BioAge = agentPatient.CurrentBioAge,
                    BioAgeState = agentPatient.CurrentAgeRang,
                    Timestamp = DateTime.Now //TODO переименовать query под currentState
                };
                return state;
            }
            catch(AgentNotFoundException ex)
            {
                throw new GetAgingStateException("Agent was not found", ex);
            }
        }
    }
}
