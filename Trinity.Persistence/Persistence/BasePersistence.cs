using System.Threading.Tasks;
using Trinity.Persistence.Contexts;
using Trinity.Persistence.Contracts;

namespace Trinity.Persistence.Persistence
{
  public class BasePersistence<D> : StaticPersistence<D>, IBasePersistence<D> where D : class
  {
    public BasePersistence(IMongoDbContext mongoDbContext) : base(mongoDbContext) { }

    public async Task<bool> Add(D entity)
    {
      await this.MongoCollection.InsertOneAsync(entity);
      return true;
    }
  }
}
