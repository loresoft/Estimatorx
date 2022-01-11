using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using MongoDB.Abstracts;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Estimatorx.Data.Mongo.Security
{
    public class UserRepository : MongoRepository<User, string>, IUserRepository
    {
        public UserRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public IQueryable<User> FindAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            return Collection
                .AsQueryable()
                .Where(e => keys.Contains(e.Id));
        }

        public IQueryable<User> OrganizationMembers(string organizationId)
        {
            return Collection
                .AsQueryable()
                .Where(u => u.Organizations.Contains(organizationId));
        }


        protected override string EntityKey(User entity)
        {
            return entity.Id;
        }

        protected override Expression<Func<User, bool>> KeyExpression(string key)
        {
            return user => user.Id == key;
        }


        protected override void BeforeInsert(User entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(User entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            entity.Updated = DateTime.Now;

            entity.UserName = entity.Email.ToLowerInvariant();

            base.BeforeUpdate(entity);
        }

        protected override void EnsureIndexes(IMongoCollection<User> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.Indexes.CreateOne(
                new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(s => s.UserName),
                    new CreateIndexOptions { Unique = true }
                )
            );

            mongoCollection.Indexes.CreateOne(
                new CreateIndexModel<User>(
                    Builders<User>.IndexKeys.Ascending(s => s.Email),
                    new CreateIndexOptions { Unique = true }
                )
            );
        }
    }
}
