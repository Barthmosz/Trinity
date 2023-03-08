using MongoDB.Driver;
using Trinity.Persistence.ConnectionConfig;

namespace Trinity.Persistence.Contexts
{
  public class MongoDbContext : IMongoDbContext
  {
    private readonly IConnectionConfig connectionConfig;

    public MongoDbContext(IConnectionConfig connectionConfig)
    {
      this.connectionConfig = connectionConfig;
    }

    public IMongoCollection<T> GetCollection<T>()
    {
      return this.connectionConfig.MongoDatabase.GetCollection<T>(typeof(T).Name);
    }
  }
}
