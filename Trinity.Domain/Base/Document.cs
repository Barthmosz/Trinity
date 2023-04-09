using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Trinity.Domain.Base
{
    public class Document : IDocument
    {
        public Document()
        {
            Id = Guid.NewGuid().ToString();
            RegistrationDate = DateTime.Now;
            LastModifiedDate = DateTime.Now;
        }

        [BsonId]
        [BsonRequired]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; private set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime RegistrationDate { get; private set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastModifiedDate { get; set; }
    }
}
