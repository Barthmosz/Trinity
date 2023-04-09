using MongoDB.Driver;
using Trinity.Persistence.ConnectionConfig;

namespace Trinity.Persistence.Contexts
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IConnectionConfig ConnectionConfig;

        public MongoDbContext(IConnectionConfig connectionConfig)
        {
            ConnectionConfig = connectionConfig;
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return ConnectionConfig.MongoDatabase.GetCollection<T>(typeof(T).Name);
        }
    }
}
