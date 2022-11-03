using Agents.API.Data.Database;
using Agents.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Repository
{
    public class AgingStatesRepository : Repository<AgingState>, IAgingStatesRepository
    {
        public AgingStatesRepository(AgentsDbContext agentsDbContext) : base(agentsDbContext)
        {
        }

        public async Task<AgingState> GetStateAsync(int patientId, DateTime timeStamp)
        {
            return await AgentsDbContext.AgingStates.FirstOrDefaultAsync(x => x.PatientId == patientId && x.Timestamp == timeStamp);
        }


        public async Task<AgingState> AddState(AgingState agingState, bool isOverride)
        {
            IExecutionStrategy strategy = AgentsDbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                AgingState? state = await AgentsDbContext.AgingStates
                .FirstOrDefaultAsync(x => x.PatientId == agingState.PatientId && x.Timestamp == agingState.Timestamp);
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
                        throw new AddAgingStateException("", ex);
                    }
                }
            });
        }
    }
}
