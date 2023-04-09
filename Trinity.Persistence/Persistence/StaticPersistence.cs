using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Persistence.Contexts;
using Trinity.Persistence.Contracts;

namespace Trinity.Persistence.Persistence
{
    public class StaticPersistence<D> : BasePersistence<D>, IStaticPersistence<D> where D : class
    {
        private const string IdKey = "_id";

        public StaticPersistence(IMongoDbContext mongoDbContext) : base(mongoDbContext) { }

        public async Task<IEnumerable<D>> GetAllAsync()
        {
            return await this.mongoCollection.Find(Builders<D>.Filter.Empty).ToListAsync();
        }

        public async Task<D?> GetByIdAsync(string id)
        {
            IAsyncCursor<D> entity = await this.mongoCollection.FindAsync(Builders<D>.Filter.Eq(IdKey, id));
            return entity.FirstOrDefault();
        }

        public async Task<D?> GetByEmailAsync(string email)
        {
            IAsyncCursor<D> entity = await this.mongoCollection.FindAsync(Builders<D>.Filter.Eq("Email", email));
            return entity.FirstOrDefault();
        }
    }
}
