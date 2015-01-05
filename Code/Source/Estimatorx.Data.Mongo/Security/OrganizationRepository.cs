using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Estimatorx.Data.Mongo.Security
{
    public class OrganizationRepository : MongoRepository<Organization, string>, IOrganizationRepository
    {
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

            return _collection.Value
                .AsQueryable()
                .Where(e => e.Id.In(keys));
        }


        public override string EntityKey(Organization entity)
        {
            return entity.Id;
        }

        protected override BsonValue ConvertKey(string key)
        {
            return ConvertString(key);
        }


        protected override void BeforeInsert(Organization entity)
        {
            entity.Created = DateTime.Now;
            entity.Creator = UserName.Current();
            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

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


        protected override void EnsureIndexes(MongoCollection<Organization> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.CreateIndex(
                IndexKeys<Organization>.Ascending(s => s.Name),
                IndexOptions.SetUnique(true)
            );
        }
    }
}
