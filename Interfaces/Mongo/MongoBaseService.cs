using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Interfaces.Mongo
{
    public abstract class MongoBaseService<T>
    {
        protected readonly IMongoCollection<T> _collection;

        public IMongoCollection<T> Collection => _collection;

        public MongoBaseService(MongoService mongo, string collectionName)
        {
            _collection = mongo.Database.GetCollection<T>(collectionName);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            return await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> All()
        {
            return (await _collection.FindAsync((T p) => true)).ToEnumerable();
        }

        public async Task<IEnumerable<T>> Query(Expression<Func<T, bool>> filter)
        {
            return (await _collection.FindAsync(filter)).ToEnumerable();
        }

        public MongoQuery<T> Query()
        {
            return new MongoQuery<T>(_collection);
        }

        public Task Insert(T doc)
        {
            return _collection.InsertOneAsync(doc);
        }

        public MongoUpdater<T> Update()
        {
            return new MongoUpdater<T>(_collection);
        }

        public MongoUpdater<T> Update(Expression<Func<T, bool>> filter)
        {
            return new MongoUpdater<T>(_collection).Where(filter);
        }

        public Task Delete(Expression<Func<T, bool>> filter)
        {
            return _collection.DeleteManyAsync(filter);
        }
    }
}
