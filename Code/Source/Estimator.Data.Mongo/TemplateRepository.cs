using System;
using Estimator.Core;
using Estimator.Core.Providers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Estimator.Data.Mongo
{
    public class TemplateRepository 
        : MongoRepository<Template, Guid>, ITemplateRepository
    {
        public TemplateRepository()
            : this("EstimatorMongoDB")
        {
        }

        public TemplateRepository(string connectionName)
            : base(connectionName)
        {
        }

        public TemplateRepository(MongoUrl mongoUrl)
            : base(mongoUrl)
        {
        }

        public override Guid EntityKey(Template entity)
        {
            return entity.Id;
        }
        protected override BsonValue ConvertKey(Guid key)
        {
            return ConvertGuid(key);
        }
        protected override void BeforeInsert(Template entity)
        {
            entity.SysCreateDate = DateTime.Now;

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Template entity)
        {
            if (entity.SysCreateDate == DateTime.MinValue)
                entity.SysCreateDate = DateTime.Now;

            entity.SysUpdateDate = DateTime.Now;
            base.BeforeUpdate(entity);
        }

        protected override void EnsureIndexes(MongoCollection<Template> mongoCollection)
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