#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.CommandQuery.EntityFramework;

using Microsoft.Extensions.DependencyInjection;

using Context = EstimatorX.Data;
using Entities = EstimatorX.Data.Entities;

namespace EstimatorX.Domain;

public static class UserLoginServiceRegistration
{
    [RegisterServices]
    public static void Register(IServiceCollection services)
    {
        services.AddEntityQueries<Context.EstimatorContext, Entities.UserLogin, long, Models.UserLoginReadModel>();
        services.AddEntityCommands<Context.EstimatorContext, Entities.UserLogin, long, Models.UserLoginReadModel, Models.UserLoginCreateModel, Models.UserLoginUpdateModel>();
    }
}
