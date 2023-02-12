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
    public class GetAllPatientsAgingDynamicsQueryHandler :
        IRequestHandler<GetAllPatientsAgingDynamicsQuery, List<IAgingDynamics<AgingState>>>
    {

        private readonly IDynamicAgentsRepository agentPatientsRepository;
        private readonly IMediator mediator;

        public GetAllPatientsAgingDynamicsQueryHandler(IDynamicAgentsRepository agentPatientsRepository, IMediator mediator)
        {
            this.agentPatientsRepository = agentPatientsRepository;
            this.mediator = mediator;
        }

        public async Task<List<IAgingDynamics<AgingState>>> Handle(GetAllPatientsAgingDynamicsQuery request, CancellationToken cancellationToken)
        {
            List<IAgingDynamics<AgingState>> result = new List<IAgingDynamics<AgingState>>();
            Dictionary<int, IDynamicAgent> agents = new Dictionary<int, IDynamicAgent>();
            List<Influence> influences = await mediator.Send(new GetAllInfluencesQuery()
            {
                StartTimestamp = request.StartTimestamp,
                EndTimestamp = request.EndTimestamp
            });


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
                    agingDynamics.AgentStateInInfluenceStart = 
                        await CalcAgentState(agents[influence.PatientId], influence.StartTimestamp);
                    agingDynamics.AgentStateInInfluenceEnd = 
                        await CalcAgentState(agents[influence.PatientId], influence.EndTimestamp);
                    result.Add(agingDynamics);
                }
                catch(Exception ex)
                {
                    throw new NotImplementedException(); //TODO
                }
            }
            return result;
        }


        private async Task<AgingState> CalcAgentState(IDynamicAgent agent, DateTime timestamp)
        {
            throw new NotImplementedException(); //TODO - нужна реалзиация с новым видом API
            //await agent.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties()
            //{
            //    Timestamp = timestamp
            //});

            //return new AgingState()
            //{
            //    PatientId = agent.PatientId,
            //    Age = agent.CurrentAge,
            //    BioAge = agent.CurrentBioAge,
            //    BioAgeState = agent.CurrentAgeRang,
            //    Timestamp = timestamp
            //};
        }
    }
}
