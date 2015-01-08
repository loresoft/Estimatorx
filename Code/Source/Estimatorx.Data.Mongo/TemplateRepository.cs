using System;
using System.Linq;
using System.Linq.Expressions;
using Estimatorx.Core;
using Estimatorx.Core.Providers;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Estimatorx.Data.Mongo
{
    public class TemplateRepository
        : MongoRepository<Template, string>, ITemplateRepository
    {
        public TemplateRepository()
            : this("EstimatorxMongo")
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

        public override string EntityKey(Template entity)
        {
            return entity.Id;
        }

        protected override BsonValue ConvertKey(string key)
        {
            return ConvertString(key);
        }

        protected override void BeforeInsert(Template entity)
        {
            entity.Created = DateTime.Now;
            entity.Creator = UserName.Current();
            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Template entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            if (string.IsNullOrEmpty(entity.Creator))
                entity.Creator = UserName.Current();

            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

            base.BeforeUpdate(entity);
        }

        protected override void EnsureIndexes(MongoCollection<Template> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.CreateIndex(
                IndexKeys<Template>
                    .Ascending(s => s.OrganizationId)
                    .Descending(s => s.Updated)
            );
        }


        public static Expression<Func<Template, TemplateSummary>> SelectSummary()
        {
            return p => new TemplateSummary
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                OrganizationId = p.OrganizationId,               
                Created = p.Created,
                Creator = p.Creator,
                Updated = p.Updated,
                Updater = p.Updater
            };
        }

    }
}