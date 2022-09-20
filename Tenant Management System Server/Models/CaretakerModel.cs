using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tenant_Management_System_Server.Models
{
    public class CaretakerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("FullName")]
        public string FullName { get; set; } = null!;

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [BsonElement("Address")]
        public string Address { get; set; } = null!;

        [BsonElement("Email")]
        public string? Email { get; set; }
        
    }
}
