#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.CommandQuery.EntityFramework;

using Microsoft.Extensions.DependencyInjection;

using Context = EstimatorX.Data;
using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain;

public static class OrganizationServiceRegistration
{
    [RegisterServices]
    public static void Register(IServiceCollection services)
    {
        services.AddEntityQueries<Context.EstimatorContext, Entities.Organization, long, Models.OrganizationReadModel>();
        services.AddEntityCommands<Context.EstimatorContext, Entities.Organization, long, Models.OrganizationReadModel, Models.OrganizationCreateModel, Models.OrganizationUpdateModel>();
    }
}
