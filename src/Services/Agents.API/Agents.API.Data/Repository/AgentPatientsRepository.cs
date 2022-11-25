using Agents.API.Data.Database;
using Agents.API.Entities;
using Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Repository
{
    public class AgentPatientsRepository : Repository<AgentPatient>, IAgentPatientsRepository, IAgingStatesRepository
    {
        IWebRequester webRequester;
        IDbContextFactory<AgentsDbContext> dbContextFactory;

        public AgentPatientsRepository(IDbContextFactory<AgentsDbContext> dbContextFactory, IWebRequester webRequester) : base(dbContextFactory)
        {
            this.webRequester = webRequester;
            this.dbContextFactory = dbContextFactory;
        }

        public async Task<AgentPatient> GetAgentPatient(int patientId)
        {
            using (AgentsDbContext AgentsDbContext = dbContextFactory.CreateDbContext())
            {
                AgentPatient? agentPatient = await AgentsDbContext
                    .AgentPatients.FirstOrDefaultAsync(x => x.PatientId == patientId);
                if (agentPatient == null)
                    throw new AgentNotFoundException($"Not found patient agent with patient id = {patientId}.");
                try
                {
                    agentPatient.InitWebRequester(webRequester);
                    agentPatient.InitDbRequester(
                        async (x, y) => await GetStateAsync(x, y),
                        async (x, y) => await AddState(x, y));
                    agentPatient.InitStateDiagram();
                    await agentPatient.StateDiagram.UpdateStateAsync(new AgentDetermineStateProperties());
                    return agentPatient;
                }
                catch (Exception ex)
                {
                    throw new AgentNotFoundException($"Get patient agent error", ex);
                }
            }
        }


        public async Task<AgentPatient> InitAgentPatient(IPatient patient)
        {
            using (AgentsDbContext AgentsDbContext = dbContextFactory.CreateDbContext())
            {
                if (patient == null)
                    throw new InitAgentException("patient is null");
                if (!IsCorrectPatient(patient))
                    throw new InitAgentException($"Patient is incorrect: id = {patient.MedicalHistoryNumber}, gender = {patient.Gender}.");
                try
                {
                    AgentPatient? agentPatient = await AgentsDbContext
                        .AgentPatients.FirstOrDefaultAsync(x => x.PatientId == patient.MedicalHistoryNumber);
                    if (agentPatient == null)
                    {
                        agentPatient = new AgentPatient()
                        {
                            PatientId = patient.MedicalHistoryNumber,
                            Name = patient.MedicalHistoryNumber.ToString()
                        };
                        await AgentsDbContext.AddAsync(agentPatient);
                        await AgentsDbContext.SaveChangesAsync();
                    }

                    agentPatient.InitWebRequester(webRequester);
                    agentPatient.InitDbRequester(
                        async (x, y) => await GetStateAsync(x, y),
                        async (x, y) => await AddState(x, y));
                    agentPatient.InitStateDiagram();
                    return agentPatient;
                }
                catch (Exception ex)
                {
                    throw new InitAgentException($"Init agent error.", ex);
                }
            }
        }


        private bool IsCorrectPatient(IPatient patient) => 
            patient.Gender != GenderEnum.None && patient.MedicalHistoryNumber > 0;

        public async Task<AgingState> GetStateAsync(int patientId, DateTime timeStamp)
        {
            using (AgentsDbContext AgentsDbContext = dbContextFactory.CreateDbContext())
            {
                return await AgentsDbContext.AgingStates.FirstOrDefaultAsync(x => x.PatientId == patientId && x.Timestamp == timeStamp);
            }
        }

        public async Task<AgingState> AddState(AgingState agingState, bool isOverride)
        {
            using (AgentsDbContext AgentsDbContext = dbContextFactory.CreateDbContext())
            {
                IExecutionStrategy strategy = AgentsDbContext.Database.CreateExecutionStrategy();
#warning error  a second operation was started on this context instance before a previous operation completed. this is usually caused by different threads concurrently
                AgingState? state = await GetStateAsync(agingState.PatientId, agingState.Timestamp);
                return await strategy.ExecuteAsync(async () =>
                {
                    if (state != null && !isOverride)
                        throw new AddAgingStateException($"State already exist:id={agingState.PatientId},timestamp={agingState.Timestamp}");
                    else
                    {
                        try
                        {
                            if (state != null)
                            {
                                agingState.Id = state.Id;
                                AgentsDbContext.Entry(state).CurrentValues.SetValues(agingState);
                            }
                            else
                                await AgentsDbContext.AgingStates.AddAsync(agingState);
                            await AgentsDbContext.SaveChangesAsync();
                            return state != null ? state : agingState;
                        }
                        catch (Exception ex)
                        {
#warning error //A second operation was started on this context instance before a previous operation completed. This is usually caused by different threads concurrently using the same instance of DbContext. 
                            throw new AddAgingStateException("", ex);
                        }
                    }
                });
            }
        }
    }
}
