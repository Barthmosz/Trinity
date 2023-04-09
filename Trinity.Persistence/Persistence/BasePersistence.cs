using MongoDB.Driver;
using Trinity.Persistence.Contexts;

namespace Trinity.Persistence.Persistence
{
    public class BasePersistence<D> where D : class
    {
        protected IMongoDbContext mongoDbContext;
        protected IMongoCollection<D> mongoCollection;

        public BasePersistence(IMongoDbContext mongoDbContext)
        {
            this.mongoDbContext = mongoDbContext;
            this.mongoCollection = this.mongoDbContext.GetCollection<D>();
        }
    }
}
