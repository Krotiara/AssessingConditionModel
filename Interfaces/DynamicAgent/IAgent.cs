using ASMLib.DynamicAgent;
using ASMLib.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMLib.DynamicAgent
{
    public interface IAgent
    {
        public string Id { get; set; }

        public string Affiliation { get; }

        public string AgentType { get; }

        public IAgentState CurrentState { get; set; }

        public ConcurrentDictionary<string, Property> Properties { get;}

        public ConcurrentDictionary<string, Property> Variables { get;}

        /// <summary>
        /// Содержит в себе буффер для расчета свойств агентов
        /// </summary>
        public ConcurrentDictionary<(string, DateTime), Parameter> Buffer { get; set; }

        public ConcurrentDictionary<string, IAgentState> States { get;}

        public Task<UpdateStateResult> UpdateState();

        public void UpdateVariables(IEnumerable<Property> vars);

        public void AddToBuffer(Parameter parameter);

        public T GetPropertyValue<T>(string propertyName);


        public void SetSettings(IAgentSettings sets);
    }
}
