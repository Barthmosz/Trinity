using MongoDB.Driver;

namespace Trinity.Persistence.Contexts
{
  public interface IMongoDbContext
  {
    IMongoCollection<T> GetCollection<T>();
  }
}
