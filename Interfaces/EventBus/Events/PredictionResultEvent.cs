using ASMLib.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.EventBus.Events
{
    public class PredictionResultEvent : Event
    {
        public StatePrediction StatePrediction { get; set; }

        public string ErrorMessage { get; set; }
    }
}
