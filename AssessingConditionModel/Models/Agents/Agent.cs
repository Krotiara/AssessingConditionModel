using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public abstract class Agent
    {
        public string Name { get; set; }

        public StateDiagram StateDiagram { get; set; }
      

        public abstract void InitStateDiagram();

        [DisplayName("Текущее состояние")]
        public string CurrentStateName => StateDiagram.CurrentState.Name;

    }
}
