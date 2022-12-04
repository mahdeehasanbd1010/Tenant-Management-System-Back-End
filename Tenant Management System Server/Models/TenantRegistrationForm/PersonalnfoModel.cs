using MongoDB.Bson.Serialization.Attributes;

namespace Tenant_Management_System_Server.Models.TenantRegistrationForm
{
    public class PersonalInfoModel
    {
        [BsonElement("Name")]
        public string Name { get; set; } = null!;

        [BsonElement("FatherName")]
        public string FatherName { get; set; } = null!;

        [BsonElement("DateOfBirth")]
        public string DateOfBirth { get; set; } = null!;

        [BsonElement("MaritalStatus")]
        public string MaritalStatus { get; set; } = null!;

        [BsonElement("PermanentAddress")]
        public string PermanentAddress { get; set; } = null!;

        [BsonElement("Occupation")]
        public string Occupation { get; set; } = null!;

        [BsonElement("AddressOfTheInstitutionOrWorkPlace")]
        public string AddressOfTheInstitutionOrWorkPlace { get; set; } = null!;

        [BsonElement("Religion")]
        public string Religion { get; set; } = null!;

        [BsonElement("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [BsonElement("Email")]
        public string? Email { get; set; }

        [BsonElement("NIDNumber")]
        public string NIDNumber { get; set; } = null!;

        [BsonElement("PassportNumber")]
        public string PassportNumber { get; set; } = null!;

        [BsonElement("ImageFile")]
        public string ImageFile { get; set; } = null!;

        [BsonElement("Signature")]
        public string Signature { get; set; } = null!;

    }
}
