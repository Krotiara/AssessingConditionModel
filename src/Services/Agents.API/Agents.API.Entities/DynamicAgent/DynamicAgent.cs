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

        public DynamicAgent(int observableId, IDynamicAgentInitSettings settings)
        {
#warning нестабильная инициалзация - если нет name и Id в словаре?
            //Name = settings.ActionsArgsReplaceDict[CommonArgs.Name].ToString();
            //Id = (int)settings.ActionsArgsReplaceDict[CommonArgs.ObservedId];
#warning как менять в процессе работы?
            settings.ActionsArgsReplaceDict[CommonArgs.ObservedId] = observableId;
            settings.ActionsArgsReplaceDict[CommonArgs.StartDateTime] = DateTime.Today;
            settings.ActionsArgsReplaceDict[CommonArgs.EndDateTime] = DateTime.Today;
            Settings = settings;
        }

        public int Id { get ; set ; }
        public string Name { get; set; }

        public IDynamicAgentInitSettings Settings { get; }

        public void UpdateState()
        {
#warning подразумевается, что settings уже актуализированы и вообще всегда в актуальном состоянии.
            string actions = Settings.DetermineAgentPropertiesActions;
            //TODO - передача действия в исполнитель кода.
            //TODO - актуализация параметров.
            //TODO - вызов обновления состояния.
        }
    }
}
