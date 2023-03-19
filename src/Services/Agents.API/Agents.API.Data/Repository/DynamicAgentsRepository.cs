using Agents.API.Entities;
using Agents.API.Entities.DynamicAgent;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Repository
{


#warning Репозиторий только для одного вида агентов или для разных? Если для разных, то будет конфликт ключей observableId.
    public class DynamicAgentsRepository : IDynamicAgentsRepository
    {
        private ConcurrentDictionary<AgentKey, IDynamicAgent> dynamicAgents;
#warning Из-за прокидывания ссылок здесь сделать пришлось нижние Singleton-м.
        private readonly IAgentInitSettingsProvider agentInitSettingsProvider;
        private readonly ICodeExecutor _codeExecutor;

        public DynamicAgentsRepository(ICodeExecutor codeExecutor, IAgentInitSettingsProvider agentInitSettingsProvider)
        {
            dynamicAgents = new ConcurrentDictionary<AgentKey, IDynamicAgent>();
            this.agentInitSettingsProvider = agentInitSettingsProvider;
            _codeExecutor = codeExecutor;
        }


        public IDynamicAgent InitAgent(AgentKey key, IDynamicAgentInitSettings settings)
        {
            if (!dynamicAgents.ContainsKey(key))
                dynamicAgents[key] = new DynamicAgent(key.ObservedId, key.ObservedObjectAffilation, settings, _codeExecutor);
            return dynamicAgents[key];
        }


        public IDynamicAgent GetAgent(AgentKey key)
        {
            if (!dynamicAgents.ContainsKey(key))
            {
                IDynamicAgentInitSettings initSets = agentInitSettingsProvider.GetSettingsBy(key.AgentType);
                return InitAgent(key, initSets);        
            }
            else
                return dynamicAgents[key];
        }

        public void Clear()
        {
            dynamicAgents.Clear();
        }
    }
}
