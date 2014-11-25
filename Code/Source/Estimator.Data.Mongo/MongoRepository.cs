using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estimator.Core.Providers;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Estimator.Data.Mongo
{
    public abstract class MongoRepository<TEntity, TKey> : MongoQuery<TEntity, TKey>, IEntityRepository<TEntity, TKey>
        where TEntity : class
    {
        protected MongoRepository(string connectionName)
            : base(connectionName)
        {
        }

        protected MongoRepository(MongoUrl mongoUrl)
            : base(mongoUrl)
        {
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

        public TEntity Save(TEntity entity)
        {
            return Update(entity);
        }

        public void Save(IEnumerable<TEntity> entities)
        {
            Update(entities);
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


        protected virtual void BeforeUpdate(TEntity entity)
        {

        }

        protected virtual void BeforeInsert(TEntity entity)
        {
            BeforeUpdate(entity);
        }
    }
}
