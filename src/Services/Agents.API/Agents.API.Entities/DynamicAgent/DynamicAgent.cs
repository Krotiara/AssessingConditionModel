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

        private readonly IWebRequester webRequester;
        private readonly string codeExecutorUrl;

        public DynamicAgent(int observableId, IDynamicAgentInitSettings settings, IWebRequester webRequester)
        {
#warning нестабильная инициалзация - если нет name и Id в словаре?
            //Name = settings.ActionsArgsReplaceDict[CommonArgs.Name].ToString();
            //Id = (int)settings.ActionsArgsReplaceDict[CommonArgs.ObservedId];
#warning как менять в процессе работы?
            ObservedId = observableId;
            settings.ActionsArgsReplaceDict[CommonArgs.ObservedId] = observableId;
            settings.ActionsArgsReplaceDict[CommonArgs.StartDateTime] = DateTime.Today;
            settings.ActionsArgsReplaceDict[CommonArgs.EndDateTime] = DateTime.Today;
            Settings = settings;

            this.webRequester = webRequester;
            codeExecutorUrl = Environment.GetEnvironmentVariable("CODE_EXECUTOR_API_URL");
        }

        public int Id { get ; set ; }

        public int ObservedId { get; set; }

        public string Name { get; set; }

        public IDynamicAgentInitSettings Settings { get; }

        public async void UpdateState()
        {
#warning подразумевается, что settings уже актуализированы и вообще всегда в актуальном состоянии.
            string actions = Settings.DetermineAgentPropertiesActions;
            string url = $"{codeExecutorUrl}/codeExecutor/executeCode";
#warning Нужен дебаг, что возвращается при ContentResult
            Dictionary<string, IProperty> calculatedArgs = 
                await webRequester.GetResponse<Dictionary<string, IProperty>>(url, "POST", actions);
            foreach (KeyValuePair<string, IProperty> entry in calculatedArgs)
            {
                Settings.Properties[entry.Key] = entry.Value;
            }
            await Settings.StateDiagram.UpdateStateAsync(new DetermineStateProperties(Settings.Properties));
        }
    }
}
