using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DynamicAgent
{


    public interface IDynamicAgentInitSettings
    {

        public AgentType AgentType { get; }

        Dictionary<string, IProperty> Properties { get; set; }

        public IStateDiagram StateDiagram { get; set; }

        public string InitialActions { get; }

        public string DetermineAgentPropertiesActions
        {
            get
            {
                StringBuilder sb = new StringBuilder(InitialActions);
                foreach (KeyValuePair<CommonArgs, object> pair in ActionsArgsReplaceDict)
                    if (pair.Value != null) //Мини костыль на наличие дефолтных значений.
                        sb.Replace(pair.Key.ToString(), pair.Value.ToString());
                return sb.ToString();
            }
        }

        public Dictionary<CommonArgs, object> ActionsArgsReplaceDict { get; set; }

        public T GetPropertyValue<T>(string propertyName);
    }
}
