using Agents.API.Data.Repository;
using Agents.API.Entities;
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
    public class GetAgingDynamicsQueryHandler : IRequestHandler<GetAgingDynamicsQuery, List<IAgingDynamics<AgingState>>>
    {
        private readonly IDynamicAgentsRepository agentPatientsRepository;
        private readonly IMediator mediator;

        public GetAgingDynamicsQueryHandler(IDynamicAgentsRepository agentPatientsRepository, IMediator mediator)
        {
            this.agentPatientsRepository = agentPatientsRepository;
            this.mediator = mediator;
        }

        public async Task<List<IAgingDynamics<AgingState>>> Handle(GetAgingDynamicsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IDynamicAgent agent = agentPatientsRepository.GetAgent(request.PatientId, AgentType.AgingPatient);
                List<Influence> influences = await mediator.Send(new GetPatientInfluencesQuery() //TODO нужно ли переносить в исполнитель кода?
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

                    agingDynamics.AgentStateInInfluenceStart = await mediator.Send(new GetAgingStateQuery(request.PatientId, influence.StartTimestamp));
                    agingDynamics.AgentStateInInfluenceEnd = await mediator.Send(new GetAgingStateQuery(request.PatientId, influence.EndTimestamp));

                    res.Add(agingDynamics); 
                }
                return res;
            }
            catch(AgentNotFoundException ex)
            {
                throw new GetAgingDynamicsException($"Агент для пациента с id = {request.PatientId} не был найден", ex);
            }
            catch(GetAgentException ex)
            {
                throw new GetAgingDynamicsException($"Не удалось обновить состояние агента для пациента с id = {request.PatientId}", ex);
            }
            catch(Exception ex)
            {
                throw new GetAgingDynamicsException($"Unexpected error", ex);
            }
        }
    }
}
