using System.Globalization;

using Arbiter.CommandQuery;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace EstimatorX.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // azure app service linux doesn't set default culture
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services
            .AddAuthorizationCore()
            .AddCascadingAuthenticationState()
            .AddAuthenticationStateDeserialization();

        builder.Services
            .AddEstimatorXShared()
            .AddEstimatorXClient("WebAssembly");

        builder.Services.AddEnvironmentOptions(options =>
        {
            options.BaseAddress = builder.HostEnvironment.BaseAddress;
            options.EnvironmentName = builder.HostEnvironment.Environment;
        });

        await builder.Build().RunAsync();
    }
}
