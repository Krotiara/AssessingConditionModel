using Interfaces;
using Interfaces.DynamicAgent;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Agents.API.Entities.DynamicAgent
{
    internal class AgentStateDb : IAgentState
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string AgentId { get; set; }

        public AgentType AgentType { get; set; }

        public string Name { get; set; }
        public double NumericCharacteristic { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
