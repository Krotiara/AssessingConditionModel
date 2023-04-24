﻿using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.AgentsSettings
{
    public class AgentKey : IAgentKey
    {
        public string ObservedId { get; set; }

        public string ObservedObjectAffilation { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            AgentKey key = obj as AgentKey;
            return ObservedId == key.ObservedId
                && ObservedObjectAffilation == key.ObservedObjectAffilation;
        }

        public override int GetHashCode()
        {
            return ObservedId.GetHashCode() ^ ObservedObjectAffilation.GetHashCode();
        }
    }
}
