using Agents.API.Data.Repository;
using Agents.API.Entities;
using Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Database
{
    public class AgentPatientsRepository : Repository<AgentPatient>, IAgentPatientsRepository
    {
        //private IWebRequester webRequester;
        IWebRequester webRequester;

        public AgentPatientsRepository(AgentsDbContext agentsDbContext, IWebRequester webRequester) : base(agentsDbContext)
        {
            this.webRequester = webRequester;
            //StartAgents();
        }

        public async Task<AgentPatient> GetAgentPatient(int patientId)
        {
            AgentPatient? agentPatient = AgentsDbContext
                    .AgentPatients.FirstOrDefault(x => x.PatientId == patientId);
            if (agentPatient == null)
                throw new AgentNotFoundException($"Not found patient agent with patient id = {patientId}.");
            try
            {
                agentPatient.InitWebRequester(webRequester);
                agentPatient.InitStateDiagram();
                await agentPatient.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties());
                return agentPatient;
            }
            catch(Exception ex)
            {
                throw new AgentNotFoundException($"Get patient agent error", ex);
            }
        }

        public async Task<AgentPatient> InitAgentPatient(IPatient patient)
        {
            if (patient == null)
                throw new InitAgentException("patient is null");
            try
            {
                AgentPatient? agentPatient = AgentsDbContext
                    .AgentPatients.FirstOrDefault(x => x.PatientId == patient.MedicalHistoryNumber);
                if (agentPatient == null)
                {
                    agentPatient = new AgentPatient()
                    {
                        PatientId = patient.MedicalHistoryNumber,
                        Name = patient.MedicalHistoryNumber.ToString()
                    };
                    agentPatient.InitWebRequester(webRequester);
                    agentPatient.InitStateDiagram();
                    await AgentsDbContext.AddAsync<AgentPatient>(agentPatient);
                    await AgentsDbContext.SaveChangesAsync();
                }
                return agentPatient;
            }
            catch(Exception ex)
            {
                throw new InitAgentException($"Init agent error.", ex);
            }
        }


        //public async Task StartAgents()
        //{
        //    foreach (AgentPatient agentPatient in AgentsDbContext.AgentPatients)
        //    {
        //        agentPatient.InitWebRequester(webRequester);
        //        await agentPatient.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties());
        //    }
        //}
    }
}
