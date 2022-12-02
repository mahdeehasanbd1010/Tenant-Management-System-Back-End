using MongoDB.Bson.Serialization.Attributes;

namespace Tenant_Management_System_Server.Models.TenantRegistrationForm
{
    public class DriverModel
    {
        [BsonElement("DriverName")]
        public string DriverName { get; set; } = null!;

        [BsonElement("DriverNIDNumber")]
        public string DriverNIDNumber { get; set; } = null!;

        [BsonElement("DriverPhoneNumber")]
        public string DriverPhoneNumber { get; set; } = null!;

        [BsonElement("DriverPermanentAddress")]
        public string DriverPermanentAddress { get; set; } = null!;
    }
}
