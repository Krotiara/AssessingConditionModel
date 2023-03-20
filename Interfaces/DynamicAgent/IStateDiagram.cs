using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{
    public interface IStateDiagram
    {
        public ConcurrentDictionary<string, IAgentState> States { get; }

        public IAgentState CurrentState { get; set; }

        public Func<IDetermineStateProperties, Task<IAgentState>> DetermineState { get; set; }


        public async Task UpdateStateAsync(IDetermineStateProperties determineStateProperties)
        {
            CurrentState = await DetermineState(determineStateProperties);
        }
    }
}
