using MongoDB.Driver;
using System.Threading.Tasks;
using Trinity.Persistence.Contexts;
using Trinity.Persistence.Contracts;

namespace Trinity.Persistence.Persistence
{
    public class DynamicPersistence<D> : BasePersistence<D>, IDynamicPersistence<D> where D : class
    {
        public DynamicPersistence(IMongoDbContext mongoDbContext) : base(mongoDbContext) { }

        public async Task<bool> AddAsync(D entity)
        {
            await this.mongoCollection.InsertOneAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(D entity)
        {
            string id = entity.GetType().GetProperty("Id")!.GetValue(entity)!.ToString()!;
            FilterDefinition<D> filter = Builders<D>.Filter.Eq("_id", id);
            ReplaceOneResult result = await this.mongoCollection.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            FilterDefinition<D> filter = Builders<D>.Filter.Eq("_id", id);
            DeleteResult result = await this.mongoCollection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
