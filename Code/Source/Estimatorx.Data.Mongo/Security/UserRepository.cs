using System;
using System.Collections.Generic;
using System.Linq;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Estimatorx.Data.Mongo.Security
{
    public class UserRepository : MongoRepository<User, string>, IUserRepository
    {
        public UserRepository()
            : this("EstimatorxMongo")
        {
        }

        public UserRepository(string connectionName)
            : base(connectionName)
        {
        }

        public UserRepository(MongoUrl mongoUrl)
            : base(mongoUrl)
        {
        }

        public IQueryable<User> FindAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            return _collection.Value
                .AsQueryable()
                .Where(e => e.Id.In(keys));
        }

        public IQueryable<User> OrganizationMembers(string organizationId)
        {
            return _collection.Value
                .AsQueryable()
                .Where(u => u.Organizations.Contains(organizationId));
        }


        public override string EntityKey(User entity)
        {
            return entity.Id;
        }

        protected override BsonValue ConvertKey(string key)
        {
            return ConvertString(key);
        }


        protected override void BeforeUpdate(User entity)
        {
            entity.UserName = entity.Email.ToLowerInvariant();

            base.BeforeUpdate(entity);
        }

        protected override void EnsureIndexes(MongoCollection<User> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.CreateIndex(
                IndexKeys<User>.Ascending(s => s.UserName),
                IndexOptions.SetUnique(true)
            );

            mongoCollection.CreateIndex(
                IndexKeys<User>.Ascending(s => s.Email),
                IndexOptions.SetUnique(true)
            );
        }
    }
}