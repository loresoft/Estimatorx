#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.CommandQuery.EntityFramework;
using Arbiter.Services;

using Microsoft.Extensions.DependencyInjection;

using Context = EstimatorX.Data;
using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain;

public static class ProjectServiceRegistration
{
    [RegisterServices]
    public static void Register(IServiceCollection services)
    {
        services.AddEntityQueries<Context.EstimatorContext, Entities.Project, long, Models.ProjectReadModel>();
        services.AddEntityQueries<Context.EstimatorContext, Entities.Project, long, Models.ProjectListModel>();
        services.AddEntityCommands<Context.EstimatorContext, Entities.Project, long, Models.ProjectReadModel, Models.ProjectCreateModel, Models.ProjectUpdateModel>();
    }
}
