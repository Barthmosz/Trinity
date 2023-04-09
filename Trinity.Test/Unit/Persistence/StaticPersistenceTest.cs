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
    public class StaticPersistenceTest
    {
        private IMongoDbContext MongoDbContext;
        private IStaticPersistence<Document> StaticPersistence;
        private IDocument Document;
        private IEnumerable<Document> Documents;

        [SetUp]
        public void SetUp()
        {
            Document = new Document();
            Documents = Enumerable.Repeat((Document)Document, 500).ToList();

            MongoDbContext = new MongoDbContextMock(new Mock<IMongoDatabase>());
            ((MongoDbContextMock)MongoDbContext).InitCollection(Documents);
            StaticPersistence = new StaticPersistence<Document>(MongoDbContext);
        }

        #region GetAllAsync
        [Test]
        public async Task GetAllAsyncOk()
        {
            IEnumerable<Document> result = await StaticPersistence.GetAllAsync();
            Assert.That(result, Is.EqualTo(Documents));
        }
        #endregion

        #region GetById
        [Test]
        public async Task GetByIdAsyncOk()
        {
            Document? result = await StaticPersistence.GetByIdAsync("any_id");
            Assert.That(result, Is.EqualTo(Document));
        }
        #endregion

        #region GetByEmail
        [Test]
        public async Task GetByEmailAsyncOk()
        {
            Document? result = await StaticPersistence.GetByEmailAsync("any_email@mail.com");
            Assert.That(result, Is.EqualTo(Document));
        }
        #endregion
    }
}
