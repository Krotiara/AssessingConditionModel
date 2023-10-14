using Agents.API.Data.Store;
using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Documents;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class SettingsService
    {
        private readonly SettingsStore _store;

        private readonly ConcurrentDictionary<(string, string), AgentSettings> _settings;

        public SettingsService(SettingsStore store)
        {
            _store = store;
            _settings = new();
        }


        public async Task<bool> Init()
        {
            var sets = await _store.All();
            foreach (var set in sets)
                _settings[(set.Affiliation, set.AgentType)] = set;

            return true;
        }


        public async Task Insert(IEnumerable<AgentSettings> sets)
        {
            foreach (var settings in sets)
                await Insert(settings);
        }


        public async Task Insert(AgentSettings settings)
        {
            if (_settings.ContainsKey((settings.Affiliation, settings.AgentType)))
                return;

            await _store.Insert(settings);

            _settings[(settings.Affiliation, settings.AgentType)] = settings;
        }

        public async Task<AgentSettings?> Get(string affiliation, string agentType)
        {
            if (_settings.TryGetValue((affiliation, agentType), out AgentSettings settings))
                return settings;

            settings = await _store.Get(x => x.Affiliation == affiliation && x.AgentType == agentType);
            if (settings != null)
                _settings[(settings.Affiliation, settings.AgentType)] = settings;

            return settings;
        }

        public async Task Update(string id, AgentSettings settings)
        {
            await _store.Update(x => x.Id == id)
                .Set(x => x.Affiliation, settings.Affiliation)
                .Set(x => x.AgentType, settings.AgentType)
                .Set(x => x.States, settings.States)
                .Set(x => x.StateProperties, settings.StateProperties)
                .Set(x => x.Variables, settings.Variables)
                .Set(x => x.StateResolveCode, settings.StateResolveCode)
                .Execute();
            _settings[(settings.Affiliation, settings.AgentType)] = settings;
        }
    }
}
