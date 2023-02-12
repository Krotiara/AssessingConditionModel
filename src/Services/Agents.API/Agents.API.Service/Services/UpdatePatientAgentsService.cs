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

            //12/02/2023 - реализовать через новое api
            throw new NotImplementedException();
            //foreach((int, DateTime) pair in updateInfo.UpdateInfo)
            //{
            //    try
            //    {
            //        int patientId = pair.Item1;
            //        DateTime timeStamp = pair.Item2;
            //        IDynamicAgent agent = agentPatientsRepository.GetAgent(patientId);
            //        if (agent == null)
            //            throw new AgentNotFoundException($"Agent patient with patient id = {patientId} was not found.");
            //        AgentDetermineStateProperties agentDetermineStateProperties = new AgentDetermineStateProperties() { Timestamp = timeStamp, IsNeedRecalculation = true};
            //        await agent.UpdateState(agentDetermineStateProperties);
            //        successCount++;
            //    }
            //    catch (AgentNotFoundException ex)
            //    {
            //        //TODO log
                   
            //        continue;
            //    }
            //    catch (DetermineStateException ex)
            //    {
            //        //TODO log
                    
            //        continue;
            //    }
            //}
            //return successCount;
        }
    }
}
