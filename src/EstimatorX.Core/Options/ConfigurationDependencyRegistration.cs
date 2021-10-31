using KickStart.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Core.Options;

public class ConfigurationDependencyRegistration : IDependencyInjectionRegistration
{
    public const string ConfigurationKey = "configuration";

    public void Register(IServiceCollection services, IDictionary<string, object> data)
    {
        services
            .AddOptions<HostingConfiguration>()
            .Configure<IConfiguration>((settings, configuration) => configuration
                .GetSection(HostingConfiguration.ConfigurationName)
                .Bind(settings)
            );

        services
            .AddOptions<SendGridConfiguration>()
            .Configure<IConfiguration>((settings, configuration) => configuration
                .GetSection(SendGridConfiguration.ConfigurationName)
                .Bind(settings)
            );
    }
}
