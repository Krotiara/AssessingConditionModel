using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public interface IUpdatePatientAgentsService
    {
        public Task UpdatePatientAgents(IEnumerable<int> patientIds);
    }
}
