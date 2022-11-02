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
            return AgentsDbContext.AgingStates.FirstOrDefault(x => x.PatientId == patientId && x.Timestamp == timeStamp);
        }


        public async Task<AgingState> AddState(AgingState agingState)
        {
            IExecutionStrategy strategy = AgentsDbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                AgingState? state = AgentsDbContext.AgingStates
                .FirstOrDefault(x => x.PatientId == agingState.PatientId && x.Timestamp == agingState.Timestamp);
                if (state != null)
                    throw new AddAgingStateException($"State already exist:id={agingState.PatientId},timestamp={agingState.Timestamp}");
                try
                {
                    await AgentsDbContext.AgingStates.AddAsync(agingState);
                    await AgentsDbContext.SaveChangesAsync();
                    return agingState;
                }
                catch(Exception ex)
                {
                    throw new AddAgingStateException("", ex);
                }
            });
        }
    }
}
