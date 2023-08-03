using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{
    public interface IAgent
    {
        public string Id { get; set; }

        public string Affiliation { get; }

        public AgentType AgentType { get; }

        public IAgentState CurrentState { get; set; }

        public ConcurrentDictionary<string, IProperty> Properties { get;}

        public ConcurrentDictionary<string, IProperty> Variables { get;}

        public ConcurrentDictionary<string, IAgentState> States { get;}

        public Task UpdateState();

        public void UpdateVariables(IEnumerable<IProperty> vars);

        public T GetPropertyValue<T>(string propertyName);
    }
}
