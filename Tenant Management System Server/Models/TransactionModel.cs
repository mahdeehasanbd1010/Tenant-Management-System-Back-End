using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tenant_Management_System_Server.Models
{
    public class TransactionModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("FlatId")]
        public string FlatId { get; set; } = null!;

        [BsonElement("HouseId")]
        public string HouseId { get; set; } = null!;

        [BsonElement("TenantUserName")]
        public string TenantUserName { get; set; } = null!;

        [BsonElement("HomeownerUserName")]
        public string HomeownerUserName { get; set; } = null!;

        [BsonElement("TransactionDate")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [BsonElement("TransactionYear")]
        public int TransactionYear { get; set; }

        [BsonElement("TransactionMonth")]
        public int TransactionMonth { get; set; }

        [BsonElement("transactionDateNumber")]
        public int transactionDateNumber { get; set; }

        [BsonElement("TransactionAmount")]
        public int TransactionAmount { get; set; }

    }
}
