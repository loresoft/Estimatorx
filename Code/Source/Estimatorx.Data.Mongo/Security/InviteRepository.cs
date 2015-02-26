using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Estimatorx.Data.Mongo.Security
{
    public class InviteRepository : MongoRepository<Invite, string>, IInviteRepository
    {
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

        public IQueryable<Invite> FindByOrganization(string organizationId)
        {
            return All().Where(o => o.OrganizationId == organizationId);
        }

        protected override BsonValue ConvertKey(string key)
        {
            return ConvertString(key);
        }


        protected override void BeforeInsert(Invite entity)
        {
            entity.Created = DateTime.Now;
            entity.Creator = UserName.Current();
            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

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


        protected override void EnsureIndexes(MongoCollection<Invite> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.CreateIndex(
                IndexKeys<Invite>.Ascending(s => s.OrganizationId)
            );
        }
    }
}
