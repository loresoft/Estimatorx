using System;
using System.Linq;
using System.Linq.Expressions;

using Estimatorx.Data.Mongo.Providers;

using MongoDB.Abstracts;
using MongoDB.Driver;

namespace Estimatorx.Data.Mongo
{
    public class TemplateRepository
        : MongoRepository<Template, string>, ITemplateRepository
    {
        public TemplateRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }

        public IQueryable<TemplateSummary> Summaries()
        {
            return All().Select(SelectSummary());
        }


        protected override string EntityKey(Template entity)
        {
            return entity.Id;
        }

        protected override Expression<Func<Template, bool>> KeyExpression(string key)
        {
            return template => template.Id == key;
        }


        protected override void BeforeInsert(Template entity)
        {
            entity.Created = DateTime.Now;
            entity.Creator = String.Empty;
            entity.Updated = DateTime.Now;
            entity.Updater = String.Empty;

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Template entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            if (string.IsNullOrEmpty(entity.Creator))
                entity.Creator = String.Empty;

            entity.Updated = DateTime.Now;
            entity.Updater = String.Empty;

            base.BeforeUpdate(entity);
        }


        protected override void EnsureIndexes(IMongoCollection<Template> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.Indexes.CreateOne(
                new CreateIndexModel<Template>(
                    Builders<Template>.IndexKeys
                        .Ascending(s => s.OrganizationId)
                        .Descending(s => s.Updated)
                )
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
