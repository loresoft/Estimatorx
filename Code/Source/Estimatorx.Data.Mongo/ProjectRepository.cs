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
        : MongoRepository<Project, Guid>, IProjectRepository
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
            entity.SysUpdateDate = DateTime.Now;
            entity.SysUpdateUser = UserName.Current();

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
                IsActive = p.IsActive,
                SysCreateDate = p.SysCreateDate,
                SysCreateUser = p.SysCreateUser,
                SysUpdateDate = p.SysUpdateDate,
                SysUpdateUser = p.SysUpdateUser
            };
        }

    }


}