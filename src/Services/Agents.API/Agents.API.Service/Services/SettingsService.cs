using Agents.API.Data.Store;
using Agents.API.Entities.AgentsSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Service.Services
{
    public class SettingsService
    {
        private readonly SettingsStore _store;

        public SettingsService(SettingsStore store)
        {
            _store = store;
        }


        public void Insert(string affiliation, AgentSettings settings) => _store.Insert(affiliation, settings);

        public AgentSettings? Get(AgentSettingsKey key) => _store.Get(key);
    }
}
