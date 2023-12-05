using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Core.Options;

public static class ConfigurationServiceModule
{
    [RegisterServices]
    [SuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
    public static void Register(IServiceCollection services)
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
