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
    public class GetAgingDynamicsQueryHandler : IRequestHandler<GetAgingDynamicsQuery, List<IAgingDynamics<AgingPatientState>>>
    {
        private readonly IAgentPatientsRepository agentPatientsRepository;
        private readonly IMediator mediator;

        public GetAgingDynamicsQueryHandler(IAgentPatientsRepository agentPatientsRepository, IMediator mediator)
        {
            this.agentPatientsRepository = agentPatientsRepository;
            this.mediator = mediator;
        }

        public async Task<List<IAgingDynamics<AgingPatientState>>> Handle(GetAgingDynamicsQuery request, CancellationToken cancellationToken)
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

                List<IAgingDynamics<AgingPatientState>> res = new List<IAgingDynamics<AgingPatientState>>();
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


        private async Task<AgingPatientState> CalcAgentStateInInfluenceStartAsync(AgentPatient agent, Influence influence)
        {
            await agent.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties()
            {
                StartTimestamp = influence.StartTimestamp,
                EndTimestamp = influence.StartTimestamp //Для расчета в начале воздействия берем параметры только из начала воздействия.
            });

            return new AgingPatientState()
            {
                PatientId = agent.PatientId,
                Age = agent.CurrentAge,
                BioAge = agent.CurrentBioAge,
                AgentBioAgeState = agent.CurrentAgeRang
            };
        }


        private async Task<AgingPatientState> CalcAgentStateInInfluenceEndAsync(AgentPatient agent, Influence influence)
        {
            await agent.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties()
            {
                StartTimestamp = influence.StartTimestamp,
                EndTimestamp = influence.EndTimestamp
            });

            return new AgingPatientState()
            {
                PatientId = agent.PatientId,
                Age = agent.CurrentAge,
                BioAge = agent.CurrentBioAge,
                AgentBioAgeState = agent.CurrentAgeRang
            };
        }
    }
}
