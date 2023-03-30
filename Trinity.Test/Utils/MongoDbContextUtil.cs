using MongoDB.Driver;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Trinity.Test.Utils
{
    internal static class MongoDbContextUtil
    {
        internal static void CreateMockCollection<T>(this Mock<IMongoDatabase> mongoDatabaseMock, IEnumerable<T> collectionList)
        {
            Mock<IMongoCollection<T>> mongoCollectionMock = new();
            mongoCollectionMock.InitMongoCollection(collectionList);
            mongoDatabaseMock.Setup(m => m.GetCollection<T>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(mongoCollectionMock.Object);
        }

        private static void InitMongoCollection<T>(this Mock<IMongoCollection<T>> mongoCollectionMock, IEnumerable<T> collectionList)
        {
            Mock<IAsyncCursor<T>> cursorMock = new();
            Mock<DeleteResult> deleteMock = new();
            Mock<UpdateResult> updateMock = new();
            Mock<ReplaceOneResult> replaceOneMock = new();

            cursorMock.Setup(c => c.Current).Returns(collectionList);
            cursorMock.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true)).Returns(Task.FromResult(false));
            mongoCollectionMock.Setup(m => m.FindAsync(
                It.IsAny<FilterDefinition<T>>(),
                It.IsAny<FindOptions<T, T>>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(cursorMock.Object);

            deleteMock.Setup(d => d.DeletedCount).Returns(1);
            mongoCollectionMock.Setup(m => m.DeleteOneAsync(
                It.IsAny<FilterDefinition<T>>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(deleteMock.Object);

            updateMock.Setup(u => u.ModifiedCount).Returns(1);
            mongoCollectionMock.Setup(m => m.UpdateOneAsync(
                It.IsAny<FilterDefinition<T>>(),
                It.IsAny<UpdateDefinition<T>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(updateMock.Object);

            replaceOneMock.Setup(r => r.ModifiedCount).Returns(1);
            mongoCollectionMock.Setup(m => m.ReplaceOneAsync(
                It.IsAny<FilterDefinition<T>>(),
                It.IsAny<T>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(replaceOneMock.Object);
        }
    }
}
