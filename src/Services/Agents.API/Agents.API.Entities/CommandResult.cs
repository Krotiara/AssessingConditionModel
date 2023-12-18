using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class CommandResult
    {
        public CommandResult(object result, string errorMessage = null)
        {
            Result = result;
            ErrorMessage = errorMessage;
        }

        public CommandResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public object Result { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsError => ErrorMessage != null && ErrorMessage != string.Empty;
    }
}
