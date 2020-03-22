using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Estimatorx.Core;
using Estimatorx.Core.Providers;
using Estimatorx.Core.Security;
using MongoDB.Abstracts;
using MongoDB.Driver;
using NLog.Fluent;

namespace Estimatorx.Data.Mongo
{
    public class ProjectRepository
        : MongoRepository<Project, string>, IProjectRepository
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();  

        public ProjectRepository()
            : this("EstimatorxMongo")
        {
        }

        public ProjectRepository(string connectionName)
            : base(MongoFactory.GetDatabaseFromConnectionName(connectionName))
        {
        }

        public ProjectRepository(MongoUrl mongoUrl)
            : base(MongoFactory.GetDatabaseFromMongoUrl(mongoUrl))
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

        protected override Expression<Func<Project, bool>> KeyExpression(string key)
        {
            return project => project.Id == key;
        }


        protected override void BeforeInsert(Project entity)
        {
            entity.Created = DateTime.Now;
            entity.Creator = UserName.Current();
            entity.Updated = DateTime.Now;
            entity.Updater = UserName.Current();

            _logger.Debug()
                .Message("Project '{0}' created by '{1}'", entity.Name, entity.Creator)
                .Property("Organization", entity.OrganizationId)
                .Write();

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


        protected override void EnsureIndexes(IMongoCollection<Project> mongoCollection)
        {
            base.EnsureIndexes(mongoCollection);

            mongoCollection.Indexes.CreateOne(
                new CreateIndexModel<Project>(
                    Builders<Project>.IndexKeys
                        .Ascending(s => s.OrganizationId)
                        .Descending(s => s.Updated)
                )
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
                OrganizationId = p.OrganizationId,
                Created = p.Created,
                Creator = p.Creator,
                Updated = p.Updated,
                Updater = p.Updater
            };
        }
    }


}