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
    public class ProjectRepository
        : MongoRepository<Project, string>, IProjectRepository
    {
        public ProjectRepository()
            : this("EstimatorxMongo")
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


        public IQueryable<ProjectSummary> Summaries()
        {
            return All().Select(SelectSummary());
        }
        
        public override string EntityKey(Project entity)
        {
            return entity.Id;
        }

        protected override BsonValue ConvertKey(string key)
        {
            return ConvertString(key);
        }

        protected override void BeforeInsert(Project entity)
        {
            entity.Created = DateTime.Now;
            entity.Creator = UserName.Current();
            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

            base.BeforeInsert(entity);
        }

        protected override void BeforeUpdate(Project entity)
        {
            if (entity.Created == DateTime.MinValue)
                entity.Created = DateTime.Now;

            if (string.IsNullOrEmpty(entity.Creator))
                entity.Creator = UserName.Current();

            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

            base.BeforeUpdate(entity);
        }

        protected override void EnsureIndexes(MongoCollection<Project> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.CreateIndex(
                IndexKeys<Project>
                    .Ascending(s => s.Creator)
                    .Descending(s => s.Updated)
            );
        }


        public static Expression<Func<Project, ProjectSummary>> SelectSummary()
        {
            return p => new ProjectSummary
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                HoursPerWeek = p.HoursPerWeek,
                ContingencyRate = p.ContingencyRate,
                TotalTasks = p.TotalTasks,
                TotalHours = p.TotalHours,
                TotalWeeks = p.TotalWeeks,
                ContingencyHours = p.ContingencyHours,
                ContingencyWeeks = p.ContingencyWeeks,
                Created = p.Created,
                Creator = p.Creator,
                Updated = p.Updated,
                Updater = p.Updater
            };
        }

    }


}