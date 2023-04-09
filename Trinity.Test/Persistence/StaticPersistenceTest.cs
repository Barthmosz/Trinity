using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trinity.Domain.Base;
using Trinity.Persistence.Contexts;
using Trinity.Persistence.Contracts;
using Trinity.Persistence.Persistence;
using Trinity.Test.Configs.Context;

namespace Trinity.Test.Persistence
{
    public class StaticPersistenceTest
    {
        private IMongoDbContext mongoDbContext;
        private IStaticPersistence<Document> staticPersistence;
        private IDocument document;
        private IEnumerable<Document> documents;

        [SetUp]
        public void SetUp()
        {
            this.document = new Document();
            this.documents = Enumerable.Repeat((Document)this.document, 500).ToList();

            this.mongoDbContext = new MongoDbContextMock(new Mock<IMongoDatabase>());
            ((MongoDbContextMock)this.mongoDbContext).InitCollection(this.documents);
            this.staticPersistence = new StaticPersistence<Document>(this.mongoDbContext);
        }

        #region GetAllAsync
        [Test]
        public async Task GetAllAsyncOk()
        {
            IEnumerable<Document> result = await this.staticPersistence.GetAllAsync();
            Assert.That(result, Is.EqualTo(this.documents));
        }
        #endregion

        #region GetById
        [Test]
        public async Task GetByIdAsyncOk()
        {
            Document? result = await this.staticPersistence.GetByIdAsync("any_id");
            Assert.That(result, Is.EqualTo(this.document));
        }
        #endregion

        #region GetByEmail
        [Test]
        public async Task GetByEmailAsyncOk()
        {
            Document? result = await this.staticPersistence.GetByEmailAsync("any_email@mail.com");
            Assert.That(result, Is.EqualTo(this.document));
        }
        #endregion
    }
}
