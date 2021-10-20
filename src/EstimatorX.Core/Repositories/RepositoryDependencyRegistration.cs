using System.Collections.Generic;
using KickStart.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Core.Repositories
{
    public class RepositoryDependencyRegistration : IDependencyInjectionRegistration
    {
        public void Register(IServiceCollection services, IDictionary<string, object> data)
        {
            services.AddCosmosRepository();

            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IOrganizationRepository, OrganizationRepository>();
            services.AddSingleton<IProjectRepository, ProjectRepository>();
            services.AddSingleton<ITemplateRepository, TemplateRepository>();
        }
    }
}
