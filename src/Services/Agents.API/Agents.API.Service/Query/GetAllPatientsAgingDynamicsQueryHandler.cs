using Agents.API.Data.Repository;
using Agents.API.Entities;
using Agents.API.Service.Services;
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

    public class GetAllPatientsAgingDynamicsQuery : IRequest<List<IAgingDynamics<AgingState>>>
    {
        public DateTime StartTimestamp { get; set; }

        public DateTime EndTimestamp { get; set; }
    }

    public class GetAllPatientsAgingDynamicsQueryHandler :
        IRequestHandler<GetAllPatientsAgingDynamicsQuery, List<IAgingDynamics<AgingState>>>
    {

        private readonly IDynamicAgentsRepository agentPatientsRepository;
        private readonly IDataProviderService dataProviderService;
        private readonly IMediator mediator;

        public GetAllPatientsAgingDynamicsQueryHandler(IDynamicAgentsRepository agentPatientsRepository, IMediator mediator, IDataProviderService dataProviderService)
        {
            this.agentPatientsRepository = agentPatientsRepository;
            this.dataProviderService = dataProviderService;
            this.mediator = mediator;
        }

        public async Task<List<IAgingDynamics<AgingState>>> Handle(GetAllPatientsAgingDynamicsQuery request, CancellationToken cancellationToken)
        {
            List<IAgingDynamics<AgingState>> result = new List<IAgingDynamics<AgingState>>();
            Dictionary<int, IDynamicAgent> agents = new Dictionary<int, IDynamicAgent>();

            List<Influence> influences = 
                await dataProviderService.ExecuteSystemCommand<List<Influence>>(
                    SystemCommands.GetAllInfluences, new object[] { request.StartTimestamp, request.EndTimestamp });

            //TODO parallel
            foreach(Influence influence in influences)
            {
                try
                {
                    if (!agents.ContainsKey(influence.PatientId))
                        agents[influence.PatientId] = agentPatientsRepository.GetAgent(influence.PatientId, AgentType.AgingPatient);
                    AgingDynamics agingDynamics = new AgingDynamics()
                    {
                        StartTimestamp = influence.StartTimestamp,
                        EndTimestamp = influence.EndTimestamp,
                        InfluenceType = influence.InfluenceType,
                        MedicineName = influence.MedicineName,
                        PatientId = influence.PatientId
                    };
                    agingDynamics.AgentStateInInfluenceStart = await mediator.Send(new GetAgingStateQuery(influence.PatientId, influence.StartTimestamp));
                    agingDynamics.AgentStateInInfluenceEnd = await mediator.Send(new GetAgingStateQuery(influence.PatientId, influence.EndTimestamp));
                    result.Add(agingDynamics);
                }
                catch(Exception ex)
                {
                    throw new NotImplementedException(); //TODO
                }
            }
            return result;
        }
    }
}
