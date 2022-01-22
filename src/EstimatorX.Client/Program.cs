
using System.Text.Json;
using System.Text.Json.Serialization;

using Blazored.LocalStorage;
using Blazored.Modal;

using EstimatorX.Client.Services;
using EstimatorX.Shared;
using EstimatorX.Shared.Definitions;

using FluentRest;

using LoreSoft.Blazor.Controls;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Sotsera.Blazor.Toaster.Core.Models;

namespace EstimatorX.Client;

public static class Program
{
    private const string DefaultScope = "https://loresoftsso.onmicrosoft.com/37f215f2-9769-4c8b-b1f4-b41c13450359/API.Access";

    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var services = builder.Services;

        services.AddSingleton<IContentSerializer>(sp =>
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

            return new JsonContentSerializer(options);
        });

        services
            .AddHttpClient<GatewayClient>(client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            })
            .AddHttpMessageHandler<ProgressBarHandler>()
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        services
            .AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add(DefaultScope);
                options.ProviderOptions.LoginMode = "redirect"; // "popup";
                options.ProviderOptions.Cache.CacheLocation = "sessionStorage";
            })
            .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, AccountClaimsFactory>();

        services.AddAutoMapper(typeof(AssemblyMetadata).Assembly);

        services.AddProgressBar();
        services.AddBlazoredLocalStorage();
        services.AddBlazoredModal();
        services.AddToaster(config =>
        {
            config.PositionClass = Defaults.Classes.Position.TopCenter;
            config.PreventDuplicates = true;
            config.NewestOnTop = false;
            config.MaxDisplayedToasts = 2;
        });

        services.Scan(scan => scan
            .FromAssembliesOf(typeof(Program), typeof(AssemblyMetadata))
                .AddClasses(classes => classes.AssignableTo<IServiceTransient>())
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo<IServiceScoped>())
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<IServiceSingleton>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime()
        );

        await builder.Build().RunAsync();
    }
}
