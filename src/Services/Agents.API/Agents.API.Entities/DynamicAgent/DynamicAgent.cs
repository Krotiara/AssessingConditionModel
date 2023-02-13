using Interfaces;
using Interfaces.DynamicAgent;
using Newtonsoft.Json;
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

        public async Task UpdateState()
        {
#warning подразумевается, что settings уже актуализированы и вообще всегда в актуальном состоянии.
            string actions = Newtonsoft.Json.JsonConvert.SerializeObject(Settings.DetermineAgentPropertiesActions);
            string url = $"{codeExecutorUrl}/codeExecutor/executeCode";
            Dictionary<string, AgentProperty> calculatedArgs = 
                await webRequester.GetResponse<Dictionary<string, AgentProperty>>(url, "POST", actions);
            foreach (KeyValuePair<string, AgentProperty> entry in calculatedArgs)
            {
                Settings.Properties[entry.Key] = entry.Value;
            }
            await Settings.StateDiagram.UpdateStateAsync(new DetermineStateProperties(Settings.Properties));
        }
    }
}
