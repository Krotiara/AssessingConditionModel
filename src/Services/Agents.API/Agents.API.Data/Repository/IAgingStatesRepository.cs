using Agents.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Repository
{
    public interface IAgingStatesRepository
    {
        public Task<AgingState> GetStateAsync(int patientId, DateTime timeStamp);

        public Task<AgingState> AddState(AgingState agingState);
    }
}
