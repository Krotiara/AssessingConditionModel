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


    public class SetPropertyValueException: Exception
    {
        public SetPropertyValueException(string message): base(message)
        {

        }
    }
}
