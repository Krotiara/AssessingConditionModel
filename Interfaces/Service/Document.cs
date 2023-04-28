using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Interfaces.Service
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
