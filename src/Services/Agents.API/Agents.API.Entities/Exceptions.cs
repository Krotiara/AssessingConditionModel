using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities
{
    public class GetWebResponceException: Exception
    {
        public GetWebResponceException(string message) : base(message) { }

        public GetWebResponceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class DetermineStateException : Exception
    {
        public DetermineStateException(string message) : base(message) { }

        public DetermineStateException(string message, Exception innerException) : base(message, innerException) { }
    }

}
