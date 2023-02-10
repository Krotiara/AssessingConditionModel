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
    public class DynamicAgentsRepository : IDynamicAgentsRepository
    {
        private Dictionary<int, IDynamicAgent> dynamicAgents;
        private readonly IWebRequester webRequester;

        public DynamicAgentsRepository(IWebRequester webRequester)
        {
            dynamicAgents = new Dictionary<int, IDynamicAgent>();
            this.webRequester = webRequester;
        }


        public IDynamicAgent InitAgent(int observableId, IDynamicAgentInitSettings settings)
        {
            if (dynamicAgents.ContainsKey(observableId))
                throw new InitAgentException($"Уже существует агент для отслеживаемого объекта с Id = {observableId}.");
            dynamicAgents[observableId] = new DynamicAgent(observableId, settings, webRequester);
            return dynamicAgents[observableId];
        }


        public IDynamicAgent GetAgent(int observableId)
        {
            if (!dynamicAgents.ContainsKey(observableId))
                throw new GetAgentException($"Не найден агент для объекта с Id = {observableId}.");
            return dynamicAgents[observableId];
        }

        public void Clear()
        {
            dynamicAgents.Clear();
        }
    }
}
