using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public abstract class Agent
    {
        public string Name { get; set; }

        public StateDiagram StateDiagram { get; set; }

        public abstract void InitStateDiagram();

    }
}
