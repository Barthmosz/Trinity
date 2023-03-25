using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Trinity.Domain
{
    public class Products
    {
        [BsonElement]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; } = string.Empty;

        [BsonElement]
        [BsonRepresentation(BsonType.Int32)]
        public int Quantity { get; set; }

        [BsonElement]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
    }
}
