using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Persistence.Contexts;
using Trinity.Persistence.Contracts;

namespace Trinity.Persistence.Persistence
{
  public class StaticPersistence<D> : IStaticPersistence<D> where D : class
  {
    protected IMongoDbContext MongoDbContext;
    protected IMongoCollection<D> MongoCollection;

    public StaticPersistence(IMongoDbContext mongoDbContext)
    {
      this.MongoDbContext = mongoDbContext;
      this.MongoCollection = this.MongoDbContext.GetCollection<D>();
    }

    public virtual async Task<IEnumerable<D>> GetAllAsync()
    {
      return await this.MongoCollection.Find(Builders<D>.Filter.Empty).ToListAsync();
    }
  }
}
