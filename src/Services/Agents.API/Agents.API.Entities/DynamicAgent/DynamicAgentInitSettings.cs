using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Entities.DynamicAgent
{
    public class DynamicAgentInitSettings : IDynamicAgentInitSettings
    {

        public DynamicAgentInitSettings(string actionsWithoutArgsPlacement, AgentType agentType = AgentType.Custom)
        {
            InitialActions = actionsWithoutArgsPlacement;
            AgentType = agentType;
        }

        public AgentType AgentType { get; }

        public Dictionary<string, IProperty> Properties { get; set; }

        public Dictionary<CommonArgs, object> ActionsArgsReplaceDict { get; set; }

        public string InitialActions { get; }

        public IStateDiagram StateDiagram { get; set; }


        public T GetPropertyValue<T>(string propertyName)
        {       
            if (!typeof(T).Equals(Properties[propertyName].Type) && !typeof(T).IsEnum)
                throw new GetAgentPropertyValueException($"Несоответсвие типов переданного типа и типа параметра");
            try
            {
                if(typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), Properties[propertyName].Value.ToString());
                }
                else
                    return (T)Properties[propertyName].Value;
            }
            catch(Exception ex)
            {
                throw new GetAgentPropertyValueException("Непредвиденная ошибка поулчения параметра агента", ex);
            }
        }
    }
}
