using MongoDB.Driver;

namespace Trinity.Persistence.ConnectionConfig
{
  public interface IConnectionConfig
  {
    IMongoClient MongoClient { get; }
    IMongoDatabase MongoDatabase { get; }
  }
}
