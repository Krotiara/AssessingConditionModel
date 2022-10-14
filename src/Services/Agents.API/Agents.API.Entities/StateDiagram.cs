using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class State
    {
        public State(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }

    public class StateDiagram
    {
        public Dictionary<string, State> States { get; }
        public int CurrentStateIndex { get; private set; }
        public State CurrentState { get; private set; }

        public Func<Task<State>> DetermineState { get; set; }

        public StateDiagram(Func<Task<State>> determineStateFunc)
        {
            CurrentStateIndex = 0;
            States = new Dictionary<string, State>();
            DetermineState = determineStateFunc;
        }

        public State AddState(string name)
        {
            State state = new State(name);
            States[name] = state;
            return state;
        }


        public State GetState(string name)
        {
            return States[name];
        }


        public async Task UpdateStateAsync()
        {
            CurrentState = await DetermineState();
        }
    }
}
