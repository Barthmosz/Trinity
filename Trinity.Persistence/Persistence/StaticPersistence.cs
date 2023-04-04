﻿using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trinity.Persistence.Contexts;
using Trinity.Persistence.Contracts;

namespace Trinity.Persistence.Persistence
{
    public class StaticPersistence<D> : IStaticPersistence<D> where D : class
    {
        private const string IdKey = "_id";

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

        public async Task<D?> GetByIdAsync(string id)
        {
            IAsyncCursor<D> entity = await this.MongoCollection.FindAsync(Builders<D>.Filter.Eq(IdKey, id));
            return entity.FirstOrDefault();
        }

        public async Task<D?> GetByEmailAsync(string email)
        {
            IAsyncCursor<D> entity = await this.MongoCollection.FindAsync(Builders<D>.Filter.Eq("Email", email));
            return entity.FirstOrDefault();
        }
    }
}
