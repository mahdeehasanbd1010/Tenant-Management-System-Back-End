using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tenant_Management_System_Server.Models
{
    public class TenantModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("FullName")]
        public string FullName { get; set; } = null!;

        [BsonElement("HomeownerUsername")]
        public string HomeownerUsername { get; set; } = null!;

        [BsonElement("FlatId")]
        public string FlatId { get; set; } = null!;

        [BsonElement("UserName")]
        public string UserName { get; set; } = null!;

        [BsonElement("Email")]
        public string? Email { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; } = null!;

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;
    }
}
