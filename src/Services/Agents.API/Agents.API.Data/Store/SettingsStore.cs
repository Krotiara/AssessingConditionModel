using Agents.API.Entities.AgentsSettings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Store
{
    public class SettingsStore
    {
        private readonly ConcurrentDictionary<string, PredictionModel> _settings;

        public SettingsStore()
        {
            _settings = new ConcurrentDictionary<string, PredictionModel>();
        }

        public void Insert(PredictionModel predictionModel) => _settings[predictionModel.Organization] = predictionModel;

        public PredictionModel? Get(string organization) => _settings.GetValueOrDefault(organization);
    }
}
