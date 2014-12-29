using System;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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

        public override string EntityKey(User entity)
        {
            return entity.Id;
        }

        protected override BsonValue ConvertKey(string key)
        {
            return ConvertString(key);
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