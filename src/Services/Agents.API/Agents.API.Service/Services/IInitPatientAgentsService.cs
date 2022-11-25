using Agents.API.Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public interface IInitPatientAgentsService
    {
        public Task<IList<AgentPatient>> InitPatientAgentsAsync(IList<IPatient> patients);
    }
}
