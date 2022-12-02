using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tenant_Management_System_Server.Models
{
    public class HomeownerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("UserType")]
        public string UserType { get; set; } = "Homeowner"!;

        [BsonElement("FullName")]
        public string FullName { get; set; } = null!;

        [BsonElement("UserName")]
        public string UserName { get; set; } = null!;

        [BsonElement("Email")]
        public string Email { get; set; } = null!;

        [BsonElement("Password")]
        public string Password { get; set; } = null!;

        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [BsonElement("HouseList")]
        public List<HouseModel> HouseList { get; set; } = null!;

    }
}
