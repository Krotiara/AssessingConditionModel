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


        public void Insert(PredictionModel model) => _store.Insert(model);

        public PredictionModel? Get(string organization) => _store.Get(organization);
    }
}
