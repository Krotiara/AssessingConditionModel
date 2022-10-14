using Agents.API.Data.Database;
using Agents.API.Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class InitPatientAgentsService : IInitPatientAgentsService
    {

        private readonly IAgentPatientsRepository agentPatientsRepository;

        public InitPatientAgentsService(IAgentPatientsRepository agentPatientsRepository)
        {
            this.agentPatientsRepository = agentPatientsRepository;
        }

        public async Task InitPatientAgentsAsync(IList<IPatient> patients)
        {
            foreach (IPatient patient in patients)
                await agentPatientsRepository.InitAgentPatient(patient);
        }
    }
}
