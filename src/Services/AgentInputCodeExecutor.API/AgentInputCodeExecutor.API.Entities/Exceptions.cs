using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInputCodeExecutor.API.Entities
{
    public class ParseCodeLineException: Exception
    {
        public ParseCodeLineException(string message): base(message)
        {

        }
    }


    public class GetCommandTypeMetaException : Exception
    {
        public GetCommandTypeMetaException(string message) : base(message)
        {

        }
    }


    public class GetWebResponceException : Exception
    {
        public GetWebResponceException(string message) : base(message) { }

        public GetWebResponceException(string message, Exception innerException) : base(message, innerException) { }
    }


    public class ResolveCommandActionException : Exception
    {
        public ResolveCommandActionException(string message) : base(message) { }

        public ResolveCommandActionException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class GetCommandArgsValuesException : Exception
    {
        public GetCommandArgsValuesException(string message) : base(message) { }

        public GetCommandArgsValuesException(string message, Exception innerException) : base(message, innerException) { }
    }


    public class ExecuteCodeLineException : Exception
    {
        public ExecuteCodeLineException(string message) : base(message) { }

        public ExecuteCodeLineException(string message, Exception innerException) : base(message, innerException) { }
    }
}
