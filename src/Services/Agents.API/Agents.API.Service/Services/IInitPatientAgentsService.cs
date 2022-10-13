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
        public void InitPatientAgents(IList<IPatient> patients);
    }
}
