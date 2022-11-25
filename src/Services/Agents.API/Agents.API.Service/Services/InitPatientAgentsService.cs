using Agents.API.Data.Repository;
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

        public async Task<IList<AgentPatient>> InitPatientAgentsAsync(IList<IPatient> patients)
        {
            //TODO распараллелить
            List<string> errorMessages = new List<string>();
            List<AgentPatient> agentPatients = new List<AgentPatient>();
            foreach (IPatient patient in patients)
            {
                try
                {
                    agentPatients.Add(await agentPatientsRepository.InitAgentPatient(patient));
                }
                catch(InitAgentException ex)
                {
                    errorMessages.Add($"Init agent for Patient id = {patient.MedicalHistoryNumber} - {ex.Message}");
                    continue;
                }
            }
            if (errorMessages.Count > 0)
                throw new InitAgentsRangeException(string.Join("\n", errorMessages));
            else
                return agentPatients;
        }
    }
}
