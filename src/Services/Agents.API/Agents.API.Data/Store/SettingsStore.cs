using Agents.API.Entities.AgentsSettings;
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


    public class SettingsStore
    {
        private readonly ConcurrentDictionary<AgentSettingsKey, AgentSettings> _settings;

        public SettingsStore()
        {
            _settings = new();
        }

        public void Insert(string affiliation, AgentSettings settings) => _settings[new AgentSettingsKey(affiliation, settings.AgentType)] = settings;

        public AgentSettings? Get(AgentSettingsKey key) => _settings.GetValueOrDefault(key);
    }
}
