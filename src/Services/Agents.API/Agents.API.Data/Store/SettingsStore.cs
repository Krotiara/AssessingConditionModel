using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Documents;
using Interfaces.Mongo;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Store
{
    public struct AgentSettingsKey
    {
        public AgentSettingsKey(string affiliation, string agentType)
        {
            Affiliation = affiliation;
            AgentType = agentType;
        }

        public string Affiliation { get; set; }

        public string AgentType { get; set; }
    }


    public class SettingsStore : MongoBaseService<AgentSettings>
    {
        public SettingsStore(MongoService mongo) : base(mongo, "AgentsSettings")
        {
        }
    }
}
