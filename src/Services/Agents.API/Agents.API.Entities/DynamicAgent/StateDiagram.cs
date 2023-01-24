using Agents.API.Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class StateDiagram : IStateDiagram
    {

        public StateDiagram(IEnumerable<IAgentState> states, 
            Func<IDetermineStateProperties, Task<IAgentState>> determineStateFunc)
        {
            States = new Dictionary<string, IAgentState>();
            foreach (IAgentState state in states)
                States[state.Name] = state;
            CurrentState = states.First();
            DetermineState = determineStateFunc;
        }

        public Dictionary<string, IAgentState> States { get; }
        public IAgentState CurrentState { get; set; }
        public Func<IDetermineStateProperties, Task<IAgentState>> DetermineState { get; set; }
    }
}
