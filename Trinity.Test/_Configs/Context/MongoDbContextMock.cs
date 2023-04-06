using MongoDB.Driver;
using Moq;
using System.Collections.Generic;
using Trinity.Persistence.ConnectionConfig;
using Trinity.Persistence.Contexts;
using Trinity.Test.Configs.Utils;

namespace Trinity.Test.Configs.Context
{
    public class MongoDbContextMock : MongoDbContext
    {
        private readonly Mock<IMongoDatabase> mongoDatabaseMock;

        public MongoDbContextMock(Mock<IMongoDatabase> mongoDatabaseMock) : base(GetConnectionConfig(mongoDatabaseMock))
        {
            this.mongoDatabaseMock = mongoDatabaseMock;
        }

        internal static IConnectionConfig GetConnectionConfig(Mock<IMongoDatabase> mongoDatabaseMock)
        {
            Mock<IConnectionConfig> connectionMock = new();
            Mock<IMongoClient> mongoClientMock = new();

            connectionMock.Setup(c => c.MongoDatabase).Returns(mongoDatabaseMock.Object);

            return connectionMock.Object;
        }

        public void InitCollection<T>(IEnumerable<T> initialList)
        {
            mongoDatabaseMock.CreateMockCollection(initialList);
        }
    }
}
