using ASMLib.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.Requests.Responce
{
    public class GetAgentStateResponce
    {
        public IAgentState AgentState { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsError => ErrorMessage != null && ErrorMessage != string.Empty;
    }
}
