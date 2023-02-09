using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class DynamicAgentInitSettings : IDynamicAgentInitSettings
    {

        private readonly string actionsWithoutArgsPlacement;

        public DynamicAgentInitSettings(string actionsWithoutArgsPlacement)
        {
            this.actionsWithoutArgsPlacement = actionsWithoutArgsPlacement;
        }

        public Dictionary<string, IProperty> Properties { get; set; }

        public Dictionary<CommonArgs, object> ActionsArgsReplaceDict { get; set; }

        public string DetermineAgentPropertiesActions { get
            {
                StringBuilder sb = new StringBuilder(actionsWithoutArgsPlacement);
                foreach(KeyValuePair<CommonArgs, object> pair in ActionsArgsReplaceDict)
                    if(pair.Value != null) //Мини костыль на наличие дефолтных значений.
                        sb.Replace(pair.Key.ToString(), pair.Value.ToString());
                return sb.ToString();
            }
        }

        public IStateDiagram StateDiagram { get; set; }
    }
}
