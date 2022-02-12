using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessingConditionModel.Models
{
    public class StateDetermineException : Exception
    {
        public StateDetermineException(string message):base(message)
        {

        }
    }
}
