﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Interfaces
{
    public interface IStateDiagram
    {
        public Dictionary<string, IAgentState> States { get; }

        public int CurrentStateIndex { get; set; }

        public IAgentState CurrentState { get; set; }

        public Func<IDetermineStateProperties, Task<IAgentState>> DetermineState { get; set; }


        public async Task UpdateStateAsync(IDetermineStateProperties determineStateProperties)
        {
            CurrentState = await DetermineState(determineStateProperties);
        }
    }
}
