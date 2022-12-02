using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tenant_Management_System_Server.Models
{
    public class TenantFormModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; } = null!;

        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        [BsonElement("FatherName")]
        public string FatherName { get; set; } = null!;

        [BsonElement("DateOfBirth")]
        public string DateOfBirth { get; set; } = null!;

        [BsonElement("MaritalStatus")]
        public string MaritalStatus { get; set; } = null!;

        [BsonElement("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [BsonElement("Occuaption")]
        public string Occupation { get; set; } = null!;

        [BsonElement("PresentAddress")]
        public string PresentAddress { get; set; } = null!;

        [BsonElement("PermanentAddress")]
        public string PermanentAddress { get; set; } = null!;

        [BsonElement("NIDNumber")]
        public string NIDNumber { get; set; } = null!;

        [BsonElement("PassportNumber")]
        public string PassportNumber { get; set; } = null!;

        [BsonElement("ImageFile")]
        public string ImageFile { get; set; } = null!;
    }
}
