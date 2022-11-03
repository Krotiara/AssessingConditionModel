using Agents.API.Data.Repository;
using Agents.API.Entities;
using Interfaces;

namespace Agents.API.Service.Services
{
    public class UpdatePatientAgentsService : IUpdatePatientAgentsService
    {

        private readonly IAgentPatientsRepository agentPatientsRepository;

        public UpdatePatientAgentsService(IAgentPatientsRepository agentPatientsRepository)
        {
            this.agentPatientsRepository = agentPatientsRepository;
        }

        public async Task<int> UpdatePatientAgents(IUpdatePatientsDataInfo updateInfo)
        {
            int successCount = 0;
            //TODO распараллелить
            foreach((int, DateTime) pair in updateInfo.UpdateInfo)
            {
                try
                {
                    int patientId = pair.Item1;
                    DateTime timeStamp = pair.Item2;
                    AgentPatient agent = await agentPatientsRepository.GetAgentPatient(patientId);
                    if (agent == null)
                        throw new AgentNotFoundException($"Agent patient with patient id = {patientId} was not found.");
                    AgentDetermineStateProperties agentDetermineStateProperties = new AgentDetermineStateProperties() { Timestamp = timeStamp, IsNeedRecalculation = true};
                    await agent.StateDiagram.UpdateStateAsync(agentDetermineStateProperties);
                    successCount++;
                }
                catch (AgentNotFoundException ex)
                {
                    //TODO log
                   
                    continue;
                }
                catch (DetermineStateException ex)
                {
                    //TODO log
                    
                    continue;
                }
            }
            return successCount;
        }
    }
}
