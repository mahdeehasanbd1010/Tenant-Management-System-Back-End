using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tenant_Management_System_Server.Models.TenantRegistrationForm
{
    public class TenantRegistrationFormModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; } = null!;

        [BsonElement("PersonalInfo")]
        public PersonalnfoModel PersonalInfo { get; set; } = new PersonalnfoModel();

        [BsonElement("EmergencyContact")]
        public EmergencyContactModel EmergencyContact { get; set; } = new EmergencyContactModel();

        [BsonElement("PresentAddress")]
        public PresentAddressModel PresentAddress { get; set; } = new PresentAddressModel();

        [BsonElement("Housekeeper")]
        public HousekeeperModel Housekeeper { get; set; } = new HousekeeperModel();

        [BsonElement("Driver")]
        public DriverModel Driver { get; set; } = new DriverModel();
    }
}
