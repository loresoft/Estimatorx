using System;
using System.Linq;
using System.Linq.Expressions;
using Estimatorx.Core.Providers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Estimatorx.Data.Mongo
{
    public abstract class MongoQuery<TEntity, TKey> : IEntityQuery<TEntity, TKey>
        where TEntity : class
    {
        protected Lazy<MongoCollection<TEntity>> _collection;
        private MongoUrl _mongoUrl;

        protected MongoQuery(string connectionName)
            : this(MongoUtility.GetMongoUrl(connectionName))
        {

        }

        protected MongoQuery(MongoUrl mongoUrl)
        {
            if (mongoUrl == null)
                throw new ArgumentNullException("mongoUrl");

            _mongoUrl = mongoUrl;
            _collection = new Lazy<MongoCollection<TEntity>>(CreateCollection);
        }

        public MongoCollection<TEntity> Collection
        {
            get
            {
                return _collection.Value;
            }
        }


        public TEntity Find(TKey id)
        {
            if (Equals(null, id))
                return default(TEntity);

            var value = ConvertKey(id);
            return _collection.Value
                .FindOneByIdAs<TEntity>(value);
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            return _collection.Value
                .AsQueryable()
                .Where(criteria)
                .FirstOrDefault();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            return _collection.Value
                .AsQueryable()
                .Where(criteria);
        }

        public IQueryable<TEntity> All()
        {
            return _collection.Value
                .AsQueryable();
        }

        public long Count()
        {
            return _collection.Value.Count();
        }

        public bool Exists(Expression<Func<TEntity, bool>> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            return _collection.Value
                .AsQueryable()
                .Any(criteria);
        }


        public abstract TKey EntityKey(TEntity entity);
        protected abstract BsonValue ConvertKey(TKey key);


        protected BsonValue ConvertString(string key)
        {
            if (string.Equals(null, key))
                throw new ArgumentNullException("key");


            return ObjectId.Parse(key);
        }

        protected BsonValue ConvertGuid(Guid key)
        {
            if (Guid.Empty == key)
                throw new ArgumentNullException("key");

            return new BsonString(key.ToString());
        }


        protected virtual string GetCollectionName()
        {
            return typeof(TEntity).Name;
        }

        protected virtual MongoCollection<TEntity> CreateCollection()
        {
            var client = new MongoClient(_mongoUrl);
            var server = client.GetServer();
            var database = server.GetDatabase(_mongoUrl.DatabaseName);

            string collectionName = GetCollectionName();
            var mongoCollection = CreateCollection(database, collectionName);

            EnsureIndexes(mongoCollection);

            return mongoCollection;
        }

        protected virtual MongoCollection<TEntity> CreateCollection(MongoDatabase database, string collectionName)
        {
            var mongoCollection = database.GetCollection<TEntity>(collectionName);
            return mongoCollection;
        }


        protected virtual void EnsureIndexes(MongoCollection<TEntity> mongoCollection)
        {

        }
    }
}