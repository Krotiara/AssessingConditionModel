using Agents.API.Entities.AgentsSettings;
using Agents.API.Entities.Documents;
using System.Collections.Concurrent;

namespace Agents.API.Entities.Requests
{
    public class PredictionRequest
    {
        public string Id { get; set; }

        public string Affiliation { get; set; }

        public string AgentType { get; set; }

        public List<PredictionSettings> Settings { get; set; }

        public ConcurrentDictionary<string, AgentSettings> AgentsSettings { get; set; }
    }
}
