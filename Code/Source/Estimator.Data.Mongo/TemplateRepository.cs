using System;
using System.Linq;
using System.Linq.Expressions;
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


        public IQueryable<TemplateSummary> Summaries()
        {
            return All().Select(SelectSummary());
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
            entity.SysCreateUser = UserName.Current();

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Template entity)
        {
            if (entity.SysCreateDate == DateTime.MinValue)
                entity.SysCreateDate = DateTime.Now;

            if (string.IsNullOrEmpty(entity.SysCreateUser))
                entity.SysCreateUser = UserName.Current();

            entity.SysUpdateDate = DateTime.Now;
            entity.SysUpdateUser = UserName.Current();

            base.BeforeUpdate(entity);
        }

        protected override void EnsureIndexes(MongoCollection<Template> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.CreateIndex(
                IndexKeys<Template>
                    .Ascending(s => s.SysCreateUser)
                    .Descending(s => s.SysUpdateDate)
            );
        }


        public static Expression<Func<Template, TemplateSummary>> SelectSummary()
        {
            return p => new TemplateSummary
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                IsActive = p.IsActive,
                SysCreateDate = p.SysCreateDate,
                SysCreateUser = p.SysCreateUser,
                SysUpdateDate = p.SysUpdateDate,
                SysUpdateUser = p.SysUpdateUser
            };
        }

    }
}