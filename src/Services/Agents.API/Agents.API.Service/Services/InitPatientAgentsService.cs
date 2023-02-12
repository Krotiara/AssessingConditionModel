using Agents.API.Data.Repository;
using Agents.API.Entities;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class InitPatientAgentsService : IInitPatientAgentsService
    {

        private readonly IDynamicAgentsRepository agentPatientsRepository;
        private readonly IAgentInitSettingsProvider settingsProvider;

        public InitPatientAgentsService(IDynamicAgentsRepository agentPatientsRepository,
            IAgentInitSettingsProvider agentInitSettingsProvider)
        {
            this.agentPatientsRepository = agentPatientsRepository;
            this.settingsProvider = agentInitSettingsProvider;
        }

        public async Task<IList<IDynamicAgent>> InitPatientAgentsAsync(IList<(IPatient, AgentType)> patients)
        {
            //TODO распараллелить
            List<string> errorMessages = new List<string>();
            List<IDynamicAgent> agentPatients = new List<IDynamicAgent>();

            foreach ((IPatient, AgentType) pair in patients)
            {
                try
                {
                    IDynamicAgentInitSettings settings = settingsProvider.GetSettingsBy(pair.Item2);
                    agentPatients.Add(agentPatientsRepository.InitAgent(pair.Item1.MedicalHistoryNumber, settings));
                }
                catch(InitAgentException ex)
                {
                    errorMessages.Add($"Init agent: {ex.Message}");
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
