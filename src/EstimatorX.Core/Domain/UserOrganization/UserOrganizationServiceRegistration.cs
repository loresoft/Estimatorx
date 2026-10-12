#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.CommandQuery.EntityFramework;

using Microsoft.Extensions.DependencyInjection;

using Context = EstimatorX.Data;
using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain;

public static class UserOrganizationServiceRegistration
{
    [RegisterServices]
    public static void Register(IServiceCollection services)
    {
        services.AddEntityQueries<Context.EstimatorContext, Entities.UserOrganization, long, Models.UserOrganizationReadModel>();
        services.AddEntityCommands<Context.EstimatorContext, Entities.UserOrganization, long, Models.UserOrganizationReadModel, Models.UserOrganizationCreateModel, Models.UserOrganizationUpdateModel>();
    }
}
