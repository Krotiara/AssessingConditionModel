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
    public class GetAllPatientsAgingDynamicsQueryHandler :
        IRequestHandler<GetAllPatientsAgingDynamicsQuery, List<IAgingDynamics<AgingPatientState>>>
    {

        private readonly IAgentPatientsRepository agentPatientsRepository;
        private readonly IMediator mediator;

        public GetAllPatientsAgingDynamicsQueryHandler(IAgentPatientsRepository agentPatientsRepository, IMediator mediator)
        {
            this.agentPatientsRepository = agentPatientsRepository;
            this.mediator = mediator;
        }

        public async Task<List<IAgingDynamics<AgingPatientState>>> Handle(GetAllPatientsAgingDynamicsQuery request, CancellationToken cancellationToken)
        {
            List<IAgingDynamics<AgingPatientState>> result = new List<IAgingDynamics<AgingPatientState>>();
            foreach(AgentPatient agentPatient in agentPatientsRepository.GetAll())
            {
                try
                {
                    List<IAgingDynamics<AgingPatientState>> dynamics = await mediator.Send(new GetAgingDynamicsQuery()
                    {
                        PatientId = agentPatient.PatientId,
                        StartTimestamp = request.StartTimestamp,
                        EndTimestamp = request.EndTimestamp
                    });
                    result.AddRange(dynamics);
                }
                catch(Exception ex)
                {
                    //TODO log
                    continue;
                }
            }
            return result;
        }
    }
}
