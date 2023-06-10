
using System.Text.Json;
using System.Text.Json.Serialization;

using Blazored.LocalStorage;
using Blazored.Modal;

using EstimatorX.Client.Services;
using EstimatorX.Shared;

using FluentRest;

using LoreSoft.Blazor.Controls;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Sotsera.Blazor.Toaster.Core.Models;

namespace EstimatorX.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var services = builder.Services;

        services.AddSingleton(sp =>
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        });

        services.AddSingleton<IContentSerializer, JsonContentSerializer>();

        services
            .AddHttpClient<GatewayClient>(client =>
            {
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            })
            .AddHttpMessageHandler<ProgressBarHandler>()
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        var defaultScope = builder.Configuration.GetValue<string>("DefaultScope");

        services
            .AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add(defaultScope);
                options.ProviderOptions.LoginMode = "popup"; // "popup"  "redirect";
                options.ProviderOptions.Cache.CacheLocation = "sessionStorage";
            })
            .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, AccountClaimsFactory>();

        services.AddAutoMapper(typeof(AssemblyMetadata).Assembly);

        services.AddEstimatorXShared();
        services.AddEstimatorXClient();

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

        await builder.Build().RunAsync();
    }
}
