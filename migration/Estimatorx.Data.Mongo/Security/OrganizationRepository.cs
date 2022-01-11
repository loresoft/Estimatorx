using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using MongoDB.Abstracts;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Estimatorx.Data.Mongo.Security
{
    public class OrganizationRepository : MongoRepository<Organization, string>, IOrganizationRepository
    {
        public OrganizationRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public IQueryable<Organization> FindAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            return Collection
                .AsQueryable()
                .Where(e => keys.Contains(e.Id));
        }


        protected override string EntityKey(Organization entity)
        {
            return entity.Id;
        }

        protected override Expression<Func<Organization, bool>> KeyExpression(string key)
        {
            return organization => organization.Id == key;
        }


        protected override void BeforeInsert(Organization entity)
        {
            entity.Created = DateTime.Now;
            entity.Creator = String.Empty;
            entity.Updated = DateTime.Now;
            entity.Updater = String.Empty;

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Organization entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            if (string.IsNullOrEmpty(entity.Creator))
                entity.Creator = String.Empty;

            entity.Updated = DateTime.Now;
            entity.Updater = String.Empty;

            base.BeforeUpdate(entity);
        }


        protected override void EnsureIndexes(IMongoCollection<Organization> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.Indexes.CreateOne(
                new CreateIndexModel<Organization>(
                    Builders<Organization>.IndexKeys.Ascending(s => s.Name),
                    new CreateIndexOptions { Unique = true }
                )
            );
        }
    }
}
