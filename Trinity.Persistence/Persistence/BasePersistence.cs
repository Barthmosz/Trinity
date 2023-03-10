using MongoDB.Driver;
using SharpCompress.Common;
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

    public async Task<bool> Update(D entity)
    {
      string id = entity.GetType().GetProperty("Id")?.GetValue(entity)?.ToString() ?? string.Empty;
      FilterDefinition<D> filter = Builders<D>.Filter.Eq("_id", id);
      ReplaceOneResult result = await this.MongoCollection.ReplaceOneAsync(filter, entity);
      return result.ModifiedCount > 0;
    }

    public async Task<bool> Delete(string id)
    {
      FilterDefinition<D> filter = Builders<D>.Filter.Eq("_id", id);
      DeleteResult result = await this.MongoCollection.DeleteOneAsync(filter);
      return result.DeletedCount > 0;
    }
  }
}
