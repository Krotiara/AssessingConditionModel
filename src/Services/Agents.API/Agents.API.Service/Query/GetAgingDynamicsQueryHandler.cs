using Agents.API.Data.Repository;
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
    public class GetAgingDynamicsQueryHandler : IRequestHandler<GetAgingDynamicsQuery, List<IAgingDynamics<AgingState>>>
    {
        private readonly IAgentPatientsRepository agentPatientsRepository;
        private readonly IMediator mediator;

        public GetAgingDynamicsQueryHandler(IAgentPatientsRepository agentPatientsRepository, IMediator mediator)
        {
            this.agentPatientsRepository = agentPatientsRepository;
            this.mediator = mediator;
        }

        public async Task<List<IAgingDynamics<AgingState>>> Handle(GetAgingDynamicsQuery request, CancellationToken cancellationToken)
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

                List<IAgingDynamics<AgingState>> res = new List<IAgingDynamics<AgingState>>();
                foreach(Influence influence in influences)
                {
                    AgingDynamics agingDynamics = new AgingDynamics()
                    {
                        StartTimestamp = influence.StartTimestamp,
                        EndTimestamp = influence.EndTimestamp,
                        InfluenceType = influence.InfluenceType,
                        MedicineName = influence.MedicineName,
                        PatientId = request.PatientId,
                    };

                    agingDynamics.AgentStateInInfluenceStart = await CalcAgentStateInInfluenceStartAsync(agent, influence);
                    agingDynamics.AgentStateInInfluenceEnd = await CalcAgentStateInInfluenceEndAsync(agent, influence);

                    res.Add(agingDynamics); 
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


        private async Task<AgingState> CalcAgentStateInInfluenceStartAsync(AgentPatient agent, Influence influence)
        {
            await agent.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties()
            {
                Timestamp = influence.StartTimestamp
            });

            return new AgingState()
            {
                PatientId = agent.PatientId,
                Age = agent.CurrentAge,
                BioAge = agent.CurrentBioAge,
                BioAgeState = agent.CurrentAgeRang,
                Timestamp = influence.StartTimestamp
            };
        }


        private async Task<AgingState> CalcAgentStateInInfluenceEndAsync(AgentPatient agent, Influence influence)
        {
            await agent.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties()
            {
                Timestamp = influence.EndTimestamp
            });

            return new AgingState()
            {
                PatientId = agent.PatientId,
                Age = agent.CurrentAge,
                BioAge = agent.CurrentBioAge,
                BioAgeState = agent.CurrentAgeRang,
                Timestamp = influence.EndTimestamp
            };
        }
    }
}
