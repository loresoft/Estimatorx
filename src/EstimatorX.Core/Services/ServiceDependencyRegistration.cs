
using EstimatorX.Core.Options;

using KickStart.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SendGrid.Extensions.DependencyInjection;

namespace EstimatorX.Core.Services;

public class ServiceDependencyRegistration : IDependencyInjectionRegistration
{
    public void Register(IServiceCollection services, IDictionary<string, object> data)
    {
        services.AddSendGrid((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IOptions<SendGridConfiguration>>();
            options.ApiKey = configuration.Value.ApiKey;
        });

        services.AddSingleton<IUserCache, UserCache>();
    }
}
