using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trinity.Domain.Base;
using Trinity.Persistence.Contexts;
using Trinity.Persistence.Persistence;
using Trinity.Test.Context;

namespace Trinity.Test.Persistence
{
    public class StaticPersistenceTest
    {
        private IMongoDbContext mongoDbContext;
        private StaticPersistence<Document> staticPersistence;
        private Document document;
        private List<Document> documents;

        [SetUp]
        public void SetUp()
        {
            this.document = new();
            this.documents = Enumerable.Repeat(this.document, 500).ToList();

            this.mongoDbContext = new MongoDbContextMock(new Mock<IMongoDatabase>());
            ((MongoDbContextMock)this.mongoDbContext).InitCollection(this.documents);
            this.staticPersistence = new(this.mongoDbContext);
        }

        [Test]
        public async Task GetAllAsyncOk()
        {
            IEnumerable<Document> result = await this.staticPersistence.GetAllAsync();
            Assert.That(result, Is.EqualTo(this.documents));
        }

        [Test]
        public async Task GetByIdOk()
        {
            Document? result = await this.staticPersistence.GetByIdAsync("any_id");
            Assert.That(result, Is.EqualTo(this.document));
        }
    }
}
