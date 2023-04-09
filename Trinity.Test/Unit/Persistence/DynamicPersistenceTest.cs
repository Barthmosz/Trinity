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

namespace Trinity.Test.Unit.Persistence
{
    public class DynamicPersistenceTest
    {
        private IMongoDbContext MongoDbContext;
        private IDynamicPersistence<Document> BasePersistence;
        private IDocument Document;
        private IEnumerable<Document> Documents;

        [SetUp]
        public void SetUp()
        {
            Document = new Document();
            Documents = Enumerable.Repeat((Document)Document, 500).ToList();

            MongoDbContext = new MongoDbContextMock(new Mock<IMongoDatabase>());
            ((MongoDbContextMock)MongoDbContext).InitCollection(Documents);
            BasePersistence = new DynamicPersistence<Document>(MongoDbContext);
        }

        #region AddAsync
        [Test]
        public async Task AddAsyncOk()
        {
            bool result = await BasePersistence.AddAsync((Document)Document);
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion

        #region UpdateAsync
        [Test]
        public async Task UpdateAsyncOk()
        {
            bool result = await BasePersistence.UpdateAsync((Document)Document);
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion

        #region DeleteAsync
        [Test]
        public async Task DeleteAsyncOk()
        {
            bool result = await BasePersistence.DeleteAsync("any_id");
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion
    }
}
