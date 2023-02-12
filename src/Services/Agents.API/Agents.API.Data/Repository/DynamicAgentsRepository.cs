using Agents.API.Entities;
using Agents.API.Entities.DynamicAgent;
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
        private readonly IWebRequester webRequester;

        public DynamicAgentsRepository(IWebRequester webRequester)
        {
            dynamicAgents = new Dictionary<(int, AgentType), IDynamicAgent>();
            this.webRequester = webRequester;
        }


        public IDynamicAgent InitAgent(int observableId, IDynamicAgentInitSettings settings)
        {
            if (dynamicAgents.ContainsKey((observableId, settings.AgentType)))
                throw new InitAgentException($"Уже существует агент для отслеживаемого объекта с Id = {observableId}.");
            dynamicAgents[(observableId, settings.AgentType)] = new DynamicAgent(observableId, settings, webRequester);
            return dynamicAgents[(observableId, settings.AgentType)];
        }


        public IDynamicAgent GetAgent(int observableId, AgentType agentType)
        {
            if (!dynamicAgents.ContainsKey((observableId, agentType)))
                throw new GetAgentException($"Не найден агент для объекта с Id = {observableId}.");
            return dynamicAgents[(observableId, agentType)];
        }

        public void Clear()
        {
            dynamicAgents.Clear();
        }
    }
}
