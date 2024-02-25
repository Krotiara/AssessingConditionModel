using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.Entities
{
    public class AgentKey
    {
        public AgentKey() { }

        public AgentKey(string observedId, string observedObjectAffilation, string agentType)
        {
            ObservedId = observedId;
            ObservedObjectAffilation = observedObjectAffilation;
            AgentType = agentType;
        }

        public string ObservedId { get; set; }

        public string ObservedObjectAffilation { get; set; }

        public string AgentType { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            AgentKey key = obj as AgentKey;
            return ObservedId == key.ObservedId
                && ObservedObjectAffilation == key.ObservedObjectAffilation
                && AgentType == key.AgentType;
        }

        public override int GetHashCode()
        {
            return ObservedId.GetHashCode() ^ ObservedObjectAffilation.GetHashCode() ^ AgentType.GetHashCode();
        }
    }
}
