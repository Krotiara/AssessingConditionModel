using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class StateDiagram : IStateDiagram
    {


        public StateDiagram(ConcurrentDictionary<string, IAgentState> states, 
            Func<IDetermineStateProperties, Task<IAgentState>> determineStateFunc)
        {
            States = new ConcurrentDictionary<string, IAgentState>();
            States = states;
            CurrentState = states.First().Value;
            DetermineState = determineStateFunc;
        }

        public ConcurrentDictionary<string, IAgentState> States { get; }
        public IAgentState CurrentState { get; set; }
        public Func<IDetermineStateProperties, Task<IAgentState>> DetermineState { get; set; }
    }
}
