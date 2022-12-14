using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Tenant_Management_System_Server.Models
{
    public class FlatModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("FlatId")]
        public string FlatId { get; set; } = null!;

        [BsonElement("HouseId")]
        public string HouseId { get; set; } = null!;

        [BsonElement("HomeownerUserName")]
        public string HomeownerUserName { get; set; } = null!;

        [BsonElement("FloorNumber")]
        public int FloorNumber { get; set; } = 0;

        [BsonElement("IsRent")]
        public bool IsRent { get; set; } = false;

        [BsonElement("IsRentRequest")]
        public bool IsRentRequest { get; set; } = false;

        [BsonElement("NumberOfRoom")]
        public int NumberOfRoom { get; set; } = 0;

        [BsonElement("NumberOfWashroom")]
        public int NumberOfWashroom { get; set; } = 0;

        [BsonElement("NumberOfDiningRoom")]
        public int NumberOfDiningRoom { get; set; } = 0;

        [BsonElement("NumberOfDrawingRoom")]
        public int NumberOfDrawingRoom { get; set; } = 0;

        [BsonElement("NumberOfBalcony")]
        public int NumberOfBalcony { get; set; } = 0;

        [BsonElement("NumberOfKitchen")]
        public int NumberOfKitchen { get; set; } = 0;

        [BsonElement("Rent")]
        public int Rent { get; set; } = 0;

        [BsonElement("TenantUserName")]
        public string TenantUserName { get; set; } = null!;

        [BsonElement("UtilityBillList")]
        public List<UtilityBillModel> UtilityBillList { get; set; } = null!;

    }
}
