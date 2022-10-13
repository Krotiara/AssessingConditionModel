using Agents.API.Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Database
{
    public interface IAgentPatientsRepository: IRepository<AgentPatient>
    {
        public Task<AgentPatient> AddAgentPatient(IPatient patient);

        public Task StartAgent(AgentPatient agentPatient);
    }
}
