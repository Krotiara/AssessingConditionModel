using Agents.API.Entities;
using Agents.API.Entities.DynamicAgent;
using Agents.API.Interfaces;
using Interfaces;
using Interfaces.DynamicAgent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Repository
{

#warning Репозиторий только для одного вида агентов или для разных? Если для разных, то будет конфликт ключей observableId.
    public class DynamicAgentsRepository : IDynamicAgentsRepository
    {
        private Dictionary<(int, AgentType), IDynamicAgent> dynamicAgents;
#warning Из-за прокидывания ссылок здесь сделать пришлось нижние Singleton-м.
        private readonly IAgentInitSettingsProvider agentInitSettingsProvider;
        private readonly ICodeExecutor _codeExecutor;

        public DynamicAgentsRepository(ICodeExecutor codeExecutor, IAgentInitSettingsProvider agentInitSettingsProvider)
        {
            dynamicAgents = new Dictionary<(int, AgentType), IDynamicAgent>();
            this.agentInitSettingsProvider = agentInitSettingsProvider;
            _codeExecutor = codeExecutor;
        }


        public IDynamicAgent InitAgent(int observableId, IDynamicAgentInitSettings settings)
        {
            if (dynamicAgents.ContainsKey((observableId, settings.AgentType)))
                throw new InitAgentException($"Уже существует агент для отслеживаемого объекта с Id = {observableId}.");
            dynamicAgents[(observableId, settings.AgentType)] = new DynamicAgent(observableId, settings, _codeExecutor);
            return dynamicAgents[(observableId, settings.AgentType)];
        }


        public IDynamicAgent GetAgent(int observableId, AgentType agentType)
        {
            if (!dynamicAgents.ContainsKey((observableId, agentType)))
            {
                IDynamicAgentInitSettings initSets = agentInitSettingsProvider.GetSettingsBy(agentType);
                return InitAgent(observableId, initSets);
                
            }
            else
                return dynamicAgents[(observableId, agentType)];
        }

        public void Clear()
        {
            dynamicAgents.Clear();
        }
    }
}
