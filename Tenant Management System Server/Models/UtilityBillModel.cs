using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tenant_Management_System_Server.Models
{
    public class UtilityBillModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("BillName")]
        public string BillName { get; set; } = null!;

        [BsonElement("BillAmount")]
        public int BillAmount { get; set; } = 0;

    }
}
