using MongoDB.Bson.Serialization.Attributes;

namespace Tenant_Management_System_Server.Models.TenantRegistrationForm
{
    public class EmergencyContactModel
    {
        [BsonElement("GuardianName")]
        public string GuardianName { get; set; } = null!;

        [BsonElement("Relation")]
        public string Relation { get; set; } = null!;

        [BsonElement("GuardianAddress")]
        public string GuardianAddress { get; set; } = null!;

        [BsonElement("GuardianPhoneNumber")]
        public string GuardianPhoneNumber { get; set; } = null!;

    }
}
