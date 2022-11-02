using Agents.API.Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Repository
{
    public interface IAgentPatientsRepository : IRepository<AgentPatient>
    {
        public Task<AgentPatient> InitAgentPatient(IPatient patient);

        //public Task StartAgents();

        public Task<AgentPatient> GetAgentPatient(int patientId);
    }
}
