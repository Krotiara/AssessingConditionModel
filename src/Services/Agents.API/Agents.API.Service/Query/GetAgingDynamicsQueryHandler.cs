using Agents.API.Data.Database;
using Agents.API.Entities;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Query
{
    public class GetAgingDynamicsQueryHandler : IRequestHandler<GetAgingDynamicsQuery, List<IAgingPatientState>>
    {
        private readonly IAgentPatientsRepository agentPatientsRepository;
        private readonly IMediator mediator;

        public GetAgingDynamicsQueryHandler(IAgentPatientsRepository agentPatientsRepository, IMediator mediator)
        {
            this.agentPatientsRepository = agentPatientsRepository;
            this.mediator = mediator;
        }

        public async Task<List<IAgingPatientState>> Handle(GetAgingDynamicsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                AgentPatient agent = await agentPatientsRepository.GetAgentPatient(request.PatientId);
                List<Influence> influences = await mediator.Send(new GetPatientInfluencesQuery()
                {
                    PatientId = request.PatientId,
                    StartTimestamp = request.StartTimestamp,
                    EndTimestamp = request.EndTimestamp
                });
#warning TODO Нужна установка дат в Influence
                List<IAgingPatientState> res = new List<IAgingPatientState>();
                foreach(Influence influence in influences)
                {
                    await agent.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties()
                    {
                        StartTimestamp = influence.StartTimestamp,
                        EndTimestamp = influence.EndTimestamp
                    });
                    res.Add(new AgingPatientState()
                    {
                        PatientId = request.PatientId,
                        StartTimestamp = influence.StartTimestamp,
                        EndTimestamp = influence.EndTimestamp,
                        Age = agent.CurrentAge,
                        BioAge = agent.CurrentBioAge,
                        AgentBioAgeState = agent.CurrentAgeRang
                    });
                }
                return res;
            }
            catch(AgentNotFoundException ex)
            {
                throw new NotImplementedException(); //TODO
            }
            catch(Exception ex)
            {
                throw new NotImplementedException(); //TODO
            }

        }
    }
}
