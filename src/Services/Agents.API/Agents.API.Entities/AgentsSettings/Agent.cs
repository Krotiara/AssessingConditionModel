using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.AgentsSettings
{
    public class Agent
    {
        public string Id { get; set; }

        public string Organization { get; set; }

        public ConcurrentDictionary<string, Property> Properties { get; set; }

        public ConcurrentDictionary<string, Property> Variables { get; set; }

        public ConcurrentDictionary<string, AgentState> States { get; set; }

        public Agent(string id, string organization, AgentsSettings settings)
        {
            Id = id;
            Organization = organization;
            InitDicts(settings);
        }


        public void UpdateVariables(List<Property> vars)
        {
            foreach (Property p in vars)
                Variables[p.Name] = p;
        }


        private void InitDicts(AgentsSettings settings)
        {
            Properties = new();
            Variables = new();
            States = new();
            foreach (Property p in settings.StateProperties)
                Properties[p.Name] = p;
            foreach (Property p in settings.Variables)
                Variables[p.Name] = p;
            foreach (AgentState s in settings.States)
                States[s.Name] = s;
        }
    }
}
