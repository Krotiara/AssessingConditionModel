using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Parameters.API.Models.Documents
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
