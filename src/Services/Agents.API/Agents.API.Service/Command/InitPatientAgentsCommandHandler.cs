using Agents.API.Data.Database;
using Agents.API.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Command
{
    public class InitPatientAgentsCommandHandler : IRequestHandler<InitPatientAgentsCommand, IList<AgentPatient>>
    {

        private readonly IAgentPatientsRepository agentPatientsRepository;

        public

        public Task<IList<AgentPatient>> Handle(InitPatientAgentsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
