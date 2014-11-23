using System;
using Estimator.Core;
using Estimator.Core.Providers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Estimator.Data.Mongo
{
    public class ProjectRepository
        : MongoRepository<Project, Guid>, IProjectRepository
    {
        public ProjectRepository()
            : this("EstimatorMongoDB")
        {
        }

        public ProjectRepository(string connectionName)
            : base(connectionName)
        {
        }

        public ProjectRepository(MongoUrl mongoUrl)
            : base(mongoUrl)
        {
        }

        public override Guid EntityKey(Project entity)
        {
            return entity.Id;
        }

        protected override BsonValue ConvertKey(Guid key)
        {
            return ConvertGuid(key);
        }

        protected override void BeforeInsert(Project entity)
        {
            entity.SysCreateDate = DateTime.Now;
            entity.SysCreateUser = UserName.Current();

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Project entity)
        {
            if (entity.SysCreateDate == DateTime.MinValue)
                entity.SysCreateDate = DateTime.Now;

            if (string.IsNullOrEmpty(entity.SysCreateUser))
                entity.SysCreateUser = UserName.Current();

            entity.SysUpdateDate = DateTime.Now;
            entity.SysUpdateUser = UserName.Current();

            base.BeforeUpdate(entity);
        }

        protected override void EnsureIndexes(MongoCollection<Project> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.CreateIndex(
                IndexKeys<Project>
                    .Ascending(s => s.SysCreateUser)
                    .Descending(s => s.SysUpdateDate)
            );
        }
    }
}