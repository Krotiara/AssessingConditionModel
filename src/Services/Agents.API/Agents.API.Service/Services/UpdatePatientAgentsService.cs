using Agents.API.Data.Database;
using Agents.API.Entities;

namespace Agents.API.Service.Services
{
    public class UpdatePatientAgentsService : IUpdatePatientAgentsService
    {

        private readonly IAgentPatientsRepository agentPatientsRepository;

        public UpdatePatientAgentsService(IAgentPatientsRepository agentPatientsRepository)
        {
            this.agentPatientsRepository = agentPatientsRepository;
        }

        public async Task UpdatePatientAgents(IEnumerable<int> patientIds)
        {
            IEnumerable<AgentPatient> patients = agentPatientsRepository.GetAll();
            foreach (int patientId in patientIds)
            {
                //TODO сейчас выглядит костыльно через получение сначала всех пациентов, а затем нужного.
                AgentPatient agent = patients.FirstOrDefault(x => x.PatientId == patientId);
                if (agent == null)
                    throw new AgentNotFoundException($"Agent patient with patient id = {patientId} was not found."); //TODO
                await agent.StateDiagram.UpdateStateAsync();
            }
        }
    }
}
