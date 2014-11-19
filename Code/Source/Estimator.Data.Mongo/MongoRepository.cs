using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Estimator.Core.Providers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Estimator.Data.Mongo
{
    public abstract class MongoRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly Lazy<MongoCollection<TEntity>> _collection;
        private readonly MongoUrl _mongoUrl;

        protected MongoRepository(string connectionName)
            : this(MongoUtility.GetMongoUrl(connectionName))
        {

        }

        protected MongoRepository(MongoUrl mongoUrl)
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


        public TEntity Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            BeforeInsert(entity);
            _collection.Value.Insert<TEntity>(entity);

            return entity;
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                Insert(entity);
        }

        public void InsertBatch(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            var list = entities.ToList();
            list.ForEach(BeforeInsert);
            _collection.Value.InsertBatch<TEntity>(list);
        }


        public TEntity Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            BeforeUpdate(entity);
            _collection.Value.Save<TEntity>(entity);

            return entity;
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                Update(entity);
        }

        public void Delete(TKey key)
        {
            var value = ConvertKey(key);
            var query = Query.EQ("_id", value);
            _collection.Value.Remove(query);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
                return;

            var key = EntityKey(entity);
            Delete(key);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                Delete(entity);
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


        protected virtual void BeforeUpdate(TEntity entity)
        {

        }

        protected virtual void BeforeInsert(TEntity entity)
        {
            BeforeUpdate(entity);
        }


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

            return new BsonBinaryData(key);
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
