
using EstimatorX.Shared.Validation;

using KickStart.DependencyInjection;

using MediatR.CommandQuery.Cosmos;

using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Core;

public class CommandQueryServiceRegistration : IDependencyInjectionRegistration
{
    public void Register(IServiceCollection services, IDictionary<string, object> data)
    {
        services.AddMediator();
        services.AddAutoMapper(typeof(CommandQueryServiceRegistration).Assembly);
        services.AddValidatorsFromAssembly<CommandQueryServiceRegistration>();
        services.AddValidatorsFromAssembly<UserModelValidator>();
    }
}
