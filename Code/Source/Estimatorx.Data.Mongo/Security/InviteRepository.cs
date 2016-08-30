using System;
using System.Linq;
using System.Linq.Expressions;
using Estimatorx.Core.Security;
using MongoDB.Abstracts;
using MongoDB.Driver;
using NLog.Fluent;

namespace Estimatorx.Data.Mongo.Security
{
    public class InviteRepository : MongoRepository<Invite, string>, IInviteRepository
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public InviteRepository()
            : this("EstimatorxMongo")
        {
        }

        public InviteRepository(string connectionName)
            : base(connectionName)
        {
        }

        public InviteRepository(MongoUrl mongoUrl)
            : base(mongoUrl)
        {
        }

        public override string EntityKey(Invite entity)
        {
            return entity.Id;
        }

        protected override Expression<Func<Invite, bool>> KeyExpression(string key)
        {
            return invite => invite.Id == key;
        }


        public IQueryable<Invite> FindByOrganization(string organizationId)
        {
            return All().Where(o => o.OrganizationId == organizationId);
        }

        protected override void BeforeInsert(Invite entity)
        {
            entity.Created = DateTime.Now;
            entity.Creator = UserName.Current();
            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

            _logger.Debug()
                .Message("Invite for '{0}' created by '{1}'", entity.Email, entity.Creator)
                .Property("Organization", entity.OrganizationId)
                .Write();

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Invite entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            if (string.IsNullOrEmpty(entity.Creator))
                entity.Creator = UserName.Current();

            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

            base.BeforeUpdate(entity);
        }


        protected override void EnsureIndexes(IMongoCollection<Invite> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.Indexes.CreateOne(
                Builders<Invite>.IndexKeys.Ascending(s => s.OrganizationId)
            );
        }
    }
}
