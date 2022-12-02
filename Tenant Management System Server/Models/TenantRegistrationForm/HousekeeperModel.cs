using MongoDB.Bson.Serialization.Attributes;

namespace Tenant_Management_System_Server.Models.TenantRegistrationForm
{
    public class HousekeeperModel
    {
        [BsonElement("HousekeeperName")]
        public string HousekeeperName { get; set; } = null!;

        [BsonElement("HousekeeperNIDNumber")]
        public string HousekeeperNIDNumber { get; set; } = null!;

        [BsonElement("HousekeeperPhoneNumber")]
        public string HousekeeperPhoneNumber { get; set; } = null!;

        [BsonElement("HousekeeperPermanentAddress")]
        public string HousekeeperPermanentAddress { get; set; } = null!;
    }
}
