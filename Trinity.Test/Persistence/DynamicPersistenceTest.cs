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
    public class DynamicPersistenceTest
    {
        private IMongoDbContext mongoDbContext;
        private IDynamicPersistence<Document> basePersistence;
        private IDocument document;
        private IEnumerable<Document> documents;

        [SetUp]
        public void SetUp()
        {
            this.document = new Document();
            this.documents = Enumerable.Repeat((Document)this.document, 500).ToList();

            this.mongoDbContext = new MongoDbContextMock(new Mock<IMongoDatabase>());
            ((MongoDbContextMock)this.mongoDbContext).InitCollection(this.documents);
            this.basePersistence = new DynamicPersistence<Document>(this.mongoDbContext);
        }

        #region AddAsync
        [Test]
        public async Task AddAsyncOk()
        {
            bool result = await this.basePersistence.AddAsync((Document)this.document);
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion

        #region UpdateAsync
        [Test]
        public async Task UpdateAsyncOk()
        {
            bool result = await this.basePersistence.UpdateAsync((Document)this.document);
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion

        #region DeleteAsync
        [Test]
        public async Task DeleteAsyncOk()
        {
            bool result = await this.basePersistence.DeleteAsync("any_id");
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion
    }
}
