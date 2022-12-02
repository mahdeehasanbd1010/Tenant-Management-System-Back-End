using MongoDB.Bson.Serialization.Attributes;

namespace Tenant_Management_System_Server.Models.TenantRegistrationForm
{
    public class PresentAddressModel
    {
        [BsonElement("FlatNo")]
        public string FlatNo { get; set; } = null!;

        [BsonElement("HouseNo")]
        public string HouseNo { get; set; } = null!;

        [BsonElement("RoadNo")]
        public string RoadNo { get; set; } = null!;

        [BsonElement("Area")]
        public string Area { get; set; } = null!;

        [BsonElement("PostalCode")]
        public int PostalCode { get; set; }
    }
}
