using System;
using System.Linq;
using System.Linq.Expressions;

using MongoDB.Abstracts;
using MongoDB.Driver;

namespace Estimatorx.Data.Mongo.Security
{
    public class InviteRepository : MongoRepository<Invite, string>, IInviteRepository
    {
        public InviteRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
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
            entity.Creator = String.Empty;
            entity.Updated = DateTime.Now;
            entity.Updater = String.Empty;

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Invite entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            if (string.IsNullOrEmpty(entity.Creator))
                entity.Creator = String.Empty;

            entity.Updated = DateTime.Now;
            entity.Updater = String.Empty;

            base.BeforeUpdate(entity);
        }


        protected override void EnsureIndexes(IMongoCollection<Invite> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.Indexes.CreateOne(
                new CreateIndexModel<Invite>(
                    Builders<Invite>.IndexKeys.Ascending(s => s.OrganizationId)
                )
            );
        }

        protected override string EntityKey(Invite entity)
        {
            return entity.Id;
        }
    }
}
