using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Trinity.Domain.Base;

namespace Trinity.Domain.Entities
{
    public class Product : Document
    {
        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; } = string.Empty;

        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string Description { get; set; } = string.Empty;

        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string ImageUrl { get; set; } = string.Empty;

        [BsonRequired]
        [BsonRepresentation(BsonType.Int32)]
        public int Quantity { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Discount { get; set; } = 1;
    }
}
