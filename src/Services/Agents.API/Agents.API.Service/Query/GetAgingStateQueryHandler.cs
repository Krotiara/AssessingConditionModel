using Agents.API.Data.Database;
using Agents.API.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAgingStateQueryHandler : IRequestHandler<GetAgingStateQuery, AgingPatientState>
    {
        private readonly IAgentPatientsRepository agentPatientsRepository;

        public GetAgingStateQueryHandler(IAgentPatientsRepository agentPatientsRepository)
        {
            this.agentPatientsRepository = agentPatientsRepository;
        }

        public async Task<AgingPatientState> Handle(GetAgingStateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                AgentPatient agentPatient = await agentPatientsRepository.GetAgentPatient(request.PatientId);
                if (agentPatient == null)
                    throw new GetAgingStateException($"Agent patient for patient with id = {request.PatientId} not found.");
                AgingPatientState state = new AgingPatientState()
                {
                    PatientId = request.PatientId,
                    Age = agentPatient.CurrentAge,
                    BioAge = agentPatient.CurrentBioAge,
                    AgentBioAgeState = agentPatient.CurrentAgeRang,
#warning TODO нормальные даты по датам влияния.
                    StartTimestamp = DateTime.MinValue,
                    EndTimestamp = DateTime.MaxValue
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
