using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Estimatorx.Core.Security;
using MongoDB.Abstracts;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NLog.Fluent;

namespace Estimatorx.Data.Mongo.Security
{
    public class OrganizationRepository : MongoRepository<Organization, string>, IOrganizationRepository
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public OrganizationRepository()
            : this("EstimatorxMongo")
        {
        }

        public OrganizationRepository(string connectionName)
            : base(connectionName)
        {
        }

        public OrganizationRepository(MongoUrl mongoUrl)
            : base(mongoUrl)
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


        public override string EntityKey(Organization entity)
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
            entity.Creator = UserName.Current();
            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

            _logger.Debug()
                .Message("Organization '{0}' created by '{1}'", entity.Name, entity.Creator)
                .Property("Organization", entity.Id)
                .Write();

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Organization entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            if (string.IsNullOrEmpty(entity.Creator))
                entity.Creator = UserName.Current();

            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

            base.BeforeUpdate(entity);
        }


        protected override void EnsureIndexes(IMongoCollection<Organization> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.Indexes.CreateOne(
                Builders<Organization>.IndexKeys.Ascending(s => s.Name),
                new CreateIndexOptions { Unique = true }
            );
        }
    }
}
