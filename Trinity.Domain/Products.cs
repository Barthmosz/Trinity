using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Trinity.Domain.Base;

namespace Trinity.Domain
{
    public class Products : Document
    {
        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; } = string.Empty;

        [BsonRequired]
        [BsonRepresentation(BsonType.Int32)]
        public int Quantity { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
    }
}
