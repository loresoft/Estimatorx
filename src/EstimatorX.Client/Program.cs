
using System.Text.Json;
using System.Text.Json.Serialization;

using Blazored.LocalStorage;

using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Services;

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
                options.ProviderOptions.Cache.CacheLocation = "localStorage";
            })
            .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, AccountClaimsFactory>();


        services.AddProgressBar();
        services.AddBlazoredLocalStorage();
        services.AddToaster(config =>
        {
            config.PositionClass = Defaults.Classes.Position.TopCenter;
            config.PreventDuplicates = true;
            config.NewestOnTop = false;
            config.MaxDisplayedToasts = 2;
        });

        services
            .AddScoped<OrganizationStore>()
            .AddScoped<UserStore>()
            .AddScoped<ProjectStore>()
            .AddScoped<TemplateStore>();

        services
            .AddScoped<OrganizationRepository>()
            .AddScoped<UserRepository>()
            .AddScoped<ProjectRepository>()
            .AddScoped<TemplateRepository>();

        services
            .AddScoped<NotificationService>();

        services
            .AddScoped<IProjectBuilder, ProjectBuilder>();

        await builder.Build().RunAsync();
    }
}
