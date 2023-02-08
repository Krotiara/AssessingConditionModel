using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class DynamicAgent : IDynamicAgent
    {

        public DynamicAgent(IDynamicAgentInitSettings settings)
        {
            Name = settings.Name;
            Properties = new Dictionary<ParameterNames, IProperty>();
            foreach (IProperty prop in settings.Properties)
                Properties[prop.Name] = prop;
            StateDiagram = new StateDiagram(settings.States, settings.DetermineStateFunc);
        }

        public int Id { get ; set ; }
        public string Name { get; set; }
        public IStateDiagram StateDiagram { get ; set ; }

        public Dictionary<ParameterNames, IProperty> Properties { get; }
    }
}
