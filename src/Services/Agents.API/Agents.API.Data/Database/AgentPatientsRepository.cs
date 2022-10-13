using Agents.API.Data.Repository;
using Agents.API.Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Database
{
    public class AgentPatientsRepository : Repository<AgentPatient>, IAgentPatientsRepository
    {
        private IWebRequester webRequester;

        public AgentPatientsRepository(AgentsDbContext agentsDbContext, IWebRequester webRequester) : base(agentsDbContext)
        {
            this.webRequester = webRequester;
        }

        public Task<AgentPatient> AddAgentPatient(IPatient patient)
        {
            throw new NotImplementedException();
        }

        public Task StartAgent(AgentPatient agentPatient)
        {
            throw new NotImplementedException();
        }
    }
}
