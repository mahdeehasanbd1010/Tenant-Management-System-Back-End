using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tenant_Management_System_Server.Models
{
    public class HouseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; } = null!;

        [BsonElement("FlatList")]
        public List<FlatModel> FlatList { get; set; } = null!;

    }
}
