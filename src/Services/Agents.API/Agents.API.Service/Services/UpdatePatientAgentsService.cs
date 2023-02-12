using Agents.API.Data.Repository;
using Agents.API.Entities;
using Interfaces;
using Interfaces.DynamicAgent;

namespace Agents.API.Service.Services
{
    public class UpdatePatientAgentsService : IUpdatePatientAgentsService
    {

        private readonly IDynamicAgentsRepository agentPatientsRepository;

        public UpdatePatientAgentsService(IDynamicAgentsRepository agentPatientsRepository)
        {
            this.agentPatientsRepository = agentPatientsRepository;
        }

        public async Task<int> UpdatePatientAgents(IUpdatePatientsDataInfo updateInfo)
        {
            int successCount = 0;
            //TODO распараллелить
            foreach((int,DateTime) pair in updateInfo.UpdateInfo)
            {
                try
                {
                    int patientId = pair.Item1;
                    DateTime timeStamp = pair.Item2;
                    //TODO учет других AgentType
                    IDynamicAgent agent = agentPatientsRepository.GetAgent(patientId, AgentType.AgingPatient);
                    agent.Settings.ActionsArgsReplaceDict[CommonArgs.EndDateTime] = timeStamp;
                    agent.Settings.ActionsArgsReplaceDict[CommonArgs.StartDateTime] = DateTime.MinValue; //TODO - по идее лучше так не делать, так как захватывает все данные из бд от начала до timeStamp.
                    await agent.UpdateState();
                    successCount++;
                }
                catch(GetAgentException ex)
                {
                    //TODO log
                    continue;
                }
                catch(DetermineStateException ex)
                {
                    //TODO log
                    continue;
                }
            }
            return successCount;
        }
    }
}
