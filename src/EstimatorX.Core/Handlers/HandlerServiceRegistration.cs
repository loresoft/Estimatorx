
using Cosmos.Abstracts;

using EstimatorX.Core.Commands;
using EstimatorX.Core.Entities;
using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Models;

using KickStart.DependencyInjection;

using MediatR;
using MediatR.CommandQuery.Cosmos;
using MediatR.CommandQuery.Definitions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EstimatorX.Core.Handlers;

public class HandlerServiceRegistration : IDependencyInjectionRegistration
{
    public void Register(IServiceCollection services, IDictionary<string, object> data)
    {
        RegisterDomain<IUserRepository, User, UserModel>(services);
        RegisterDomain<IOrganizationRepository, Organization, OrganizationModel>(services);
        RegisterDomain<IProjectRepository, Project, ProjectModel>(services);
        RegisterDomain<ITemplateRepository, Template, TemplateModel>(services);

        services.TryAddTransient<IRequestHandler<UserUpdateCommand, UserModel>, UserUpdateHandler>();

    }


    private void RegisterDomain<TRepository, TEntity, TModel>(IServiceCollection services)
        where TRepository : ICosmosRepository<TEntity>
        where TEntity : class, IHaveIdentifier<string>, new()
        where TModel : class
    {
        services.AddEntityQueries<TRepository, TEntity, TModel>();
        services.AddEntityCommands<TRepository, TEntity, TModel, TModel, TModel>();
    }
}
