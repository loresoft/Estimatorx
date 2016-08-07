using System;
using System.Linq.Expressions;
using Estimatorx.Core.Security;
using MongoDB.Abstracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Estimatorx.Data.Mongo.Security
{
    public class RoleRepository : MongoRepository<Role, string>, IRoleRepository
    {
        public RoleRepository()
            : this("EstimatorxMongo")
        {
        }

        public RoleRepository(string connectionName)
            : base(connectionName)
        {
        }

        public RoleRepository(MongoUrl mongoUrl)
            : base(mongoUrl)
        {
        }

        public override string EntityKey(Role entity)
        {
            return entity.Id;
        }

        protected override Expression<Func<Role, bool>> KeyExpression(string key)
        {
            return role => role.Id == key;
        }

        protected override void BeforeInsert(Role entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Role entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            entity.Updated = DateTime.Now;

            entity.Name = entity.Name.ToLowerInvariant();

            base.BeforeUpdate(entity);
        }

        protected override void EnsureIndexes(IMongoCollection<Role> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.Indexes.CreateOne(
                Builders<Role>.IndexKeys.Ascending(s => s.Name),
                new CreateIndexOptions { Unique = true }
            );
        }
    }
}