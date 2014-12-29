using System;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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

        protected override BsonValue ConvertKey(string key)
        {
            return ConvertString(key);
        }


        protected override void EnsureIndexes(MongoCollection<Role> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.CreateIndex(
                IndexKeys<Role>.Ascending(s => s.Name),
                IndexOptions.SetUnique(true)
                );
        }
    }
}