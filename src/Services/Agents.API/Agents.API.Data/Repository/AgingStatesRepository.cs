using Agents.API.Data.Database;
using Agents.API.Entities;
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

        public AgingState GetState(int agentId, DateTime timeStamp)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception ex)
            {
                throw new NotImplementedException(); //TODO
            }
        }

        public AgingState UpdateState(AgingState agingState)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(); //TODO
            }
        }
    }
}
