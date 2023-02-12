using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{

    public enum CommonArgs
    {
        ObservedId,
        StartDateTime,
        EndDateTime
    }

    public interface IDynamicAgentInitSettings
    {

        public AgentType AgentType { get; }

        Dictionary<string, IProperty> Properties { get; set; }

        public IStateDiagram StateDiagram { get; set; }

        public string DetermineAgentPropertiesActions { get;}

        public Dictionary<CommonArgs, object> ActionsArgsReplaceDict { get; set; }
    }
}
