using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace Trinity.Persistence.ConnectionConfig
{
    [ExcludeFromCodeCoverage]
    public class ConnectionConfig : IConnectionConfig
    {
        public IMongoClient MongoClient { get; }
        public IMongoDatabase MongoDatabase { get; }

        public ConnectionConfig(IOptions<DbOptions> options)
        {
            this.MongoClient = new MongoClient(options.Value.Connection);
            this.MongoDatabase = this.MongoClient.GetDatabase(options.Value.Name);
        }
    }
}
