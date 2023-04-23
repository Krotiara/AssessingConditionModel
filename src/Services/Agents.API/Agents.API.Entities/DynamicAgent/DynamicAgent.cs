using Agents.API.Interfaces;
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
        private readonly ICodeExecutor _codeExecutor;

        public DynamicAgent(int observableId, string observedObjectAffilation, IDynamicAgentInitSettings settings, ICodeExecutor codeExecutor)
        {
#warning нестабильная инициалзация - если нет name и Id в словаре?
            //Name = settings.ActionsArgsReplaceDict[CommonArgs.Name].ToString();
            //Id = (int)settings.ActionsArgsReplaceDict[CommonArgs.ObservedId];
#warning как менять в процессе работы?
            ObservedId = observableId;
            ObservedObjectAffilation = observedObjectAffilation;
            settings.ActionsArgsReplaceDict[CommonArgs.ObservedId] = observableId;
            settings.ActionsArgsReplaceDict[CommonArgs.MedicalOrganization] = observedObjectAffilation;
            settings.ActionsArgsReplaceDict[CommonArgs.StartDateTime] = DateTime.Today;
            settings.ActionsArgsReplaceDict[CommonArgs.EndDateTime] = DateTime.Today;
            Settings = settings;

            this._codeExecutor = codeExecutor;
        }

        public int Id { get ; set ; }

        public int ObservedId { get; set; }

        public string Name { get; set; }

        public IDynamicAgentInitSettings Settings { get; }
        public string ObservedObjectAffilation { get; set; }

        public async Task UpdateState()
        {
#warning подразумевается, что settings уже актуализированы и вообще всегда в актуальном состоянии.  
            
            Dictionary<string, IProperty> calculatedArgs = await _codeExecutor.ExecuteCode(Settings.DetermineAgentPropertiesActions);
           
            foreach (KeyValuePair<string, IProperty> entry in calculatedArgs)
            {
                Settings.Properties[entry.Key] = entry.Value;
            }

            DateTime stateTimestamp = (DateTime)Settings.ActionsArgsReplaceDict[CommonArgs.EndDateTime];

            await Settings.StateDiagram.UpdateStateAsync(new DetermineStateProperties(Settings.Properties, stateTimestamp));
        }
    }
}
