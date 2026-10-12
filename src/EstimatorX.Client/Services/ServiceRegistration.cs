using System.Text.Json;

using Arbiter.CommandQuery.Options;
using Arbiter.Components;
using Arbiter.Dispatcher;

using EstimatorX.Extensions;

using LoreSoft.Blazor.Controls;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace EstimatorX.Client.Services;

public static class ServiceRegistration
{
    [RegisterServices]
    public static void Register(IServiceCollection services, ISet<string> tags)
    {
        // add cache
        services
            .AddHybridCache();

        // component libraries
        services
            .AddBlazorControls()
            .AddArbiterComponents(options => options.ShowExceptionDetails = true);

        // json options
        services
            .TryAddSingleton(_ =>
            {
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                options.AddDomainOptions();
                return options;
            });


        if (tags.Contains("WebAssembly"))
        {
            services
                .AddMessagePackDispatcher((sp, client) =>
                {
                    var hostEnvironment = sp.GetRequiredService<IOptions<EnvironmentOptions>>();
                    client.BaseAddress = hostEnvironment.Value.BaseAddress.ToUri();
                })
                .AddHttpMessageHandler<ProgressBarHandler>();
        }

        if (tags.Contains("Server"))
        {
            services.AddServerDispatcher();
        }
    }
}
