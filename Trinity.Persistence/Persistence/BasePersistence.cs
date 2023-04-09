using MongoDB.Driver;
using Trinity.Persistence.Contexts;

namespace Trinity.Persistence.Persistence
{
    public class BasePersistence<D> where D : class
    {
        protected IMongoDbContext MongoDbContext;
        protected IMongoCollection<D> MongoCollection;

        public BasePersistence(IMongoDbContext mongoDbContext)
        {
            MongoDbContext = mongoDbContext;
            MongoCollection = mongoDbContext.GetCollection<D>();
        }
    }
}
