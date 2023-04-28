using MongoDB.Driver;
using System.Linq.Expressions;

namespace Parameters.API.Services.Mongo
{
    public class MongoQuery<T>
    {
        private readonly IMongoCollection<T> _collection;

        private readonly List<FilterDefinition<T>> filters = new List<FilterDefinition<T>>();

        private SortDefinition<T> _sort;

        private int? _limit;

        public MongoQuery(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public MongoQuery<T> Where(Expression<Func<T, bool>> filter)
        {
            filters.Add(Builders<T>.Filter.Where(filter));
            return this;
        }

        public MongoQuery<T> SortAsc(Expression<Func<T, object>> field)
        {
            _sort = Builders<T>.Sort.Ascending(field);
            return this;
        }

        public MongoQuery<T> Limit(int value)
        {
            _limit = value;
            return this;
        }

        public async Task<IEnumerable<T>> Execute()
        {
            FilterDefinition<T> filter = ((filters.Count == 0) ? Builders<T>.Filter.Empty : ((filters.Count != 1) ? Builders<T>.Filter.And(filters) : filters[0]));
            FindOptions<T, T> options = new FindOptions<T, T>
            {
                Sort = _sort,
                Limit = _limit
            };
            return (await _collection.FindAsync(filter, options)).ToEnumerable();
        }
    }
}
