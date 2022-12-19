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

    public class GetAgingStateException : Exception
    {
        public GetAgingStateException(string message) : base(message) { }

        public GetAgingStateException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class AddAgingStateException : Exception
    {
        public AddAgingStateException(string message) : base(message) { }

        public AddAgingStateException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class AgentNotFoundException : Exception
    {
        public AgentNotFoundException(string message) : base(message) { }

        public AgentNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class GetAgentException : Exception
    {
        public GetAgentException(string message) : base(message) { }

        public GetAgentException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InitAgentException : Exception
    {
        public InitAgentException(string message) : base(message) { }

        public InitAgentException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InitAgentsRangeException : Exception
    {
        public InitAgentsRangeException(string message) : base(message) { }

        public InitAgentsRangeException(string message, Exception innerException) : base(message, innerException) { }
    }


    public class UpdateAgentsGroupException : Exception
    {
        public UpdateAgentsGroupException(string message) : base(message) { }

        public UpdateAgentsGroupException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class GetAgingDynamicsException : Exception
    {
        public GetAgingDynamicsException(string message) : base(message) { }

        public GetAgingDynamicsException(string message, Exception innerException) : base(message, innerException) { }
    }

}
