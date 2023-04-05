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
    public class BasePersistenceTest
    {
        private IMongoDbContext mongoDbContext;
        private BasePersistence<Document> basePersistence;
        private Document document;
        private List<Document> documents;

        [SetUp]
        public void SetUp()
        {
            this.document = new();
            this.documents = Enumerable.Repeat(this.document, 500).ToList();

            this.mongoDbContext = new MongoDbContextMock(new Mock<IMongoDatabase>());
            ((MongoDbContextMock)this.mongoDbContext).InitCollection(this.documents);
            this.basePersistence = new(this.mongoDbContext);
        }

        [Test]
        public async Task AddAsyncOk()
        {
            bool result = await this.basePersistence.AddAsync(this.document);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task UpdateAsyncOk()
        {
            bool result = await this.basePersistence.UpdateAsync(this.document);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public async Task DeleteAsyncOk()
        {
            bool result = await this.basePersistence.DeleteAsync("any_id");
            Assert.That(result, Is.EqualTo(true));
        }
    }
}
