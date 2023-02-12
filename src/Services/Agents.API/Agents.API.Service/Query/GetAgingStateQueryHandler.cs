using Agents.API.Data.Repository;
using Agents.API.Entities;
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
    public class GetAgingStateQueryHandler : IRequestHandler<GetAgingStateQuery, AgingState>
    {
        private readonly IDynamicAgentsRepository agentPatientsRepository;

        public GetAgingStateQueryHandler(IDynamicAgentsRepository agentPatientsRepository)
        {
            this.agentPatientsRepository = agentPatientsRepository;
        }

        public async Task<AgingState> Handle(GetAgingStateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IDynamicAgent agentPatient = agentPatientsRepository.GetAgent(request.PatientId, AgentType.AgingPatient);
                if (agentPatient == null)
                    throw new GetAgingStateException($"Agent patient for patient with id = {request.PatientId} not found.");


                //TODO реалзиация через новое API.
                //TODO - убрать во внешнее api
                throw new NotImplementedException();

                //AgingState state = new AgingState()
                //{
                //    PatientId = request.PatientId,
                //    Age = agentPatient.CurrentAge,
                //    BioAge = agentPatient.CurrentBioAge,
                //    BioAgeState = agentPatient.CurrentAgeRang,
                //    Timestamp = DateTime.Now //TODO переименовать query под currentState
                //};
                //return state;
            }
            catch(AgentNotFoundException ex)
            {
                throw new GetAgingStateException("Agent was not found", ex);
            }
        }
    }
}
