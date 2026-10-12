using System.Globalization;

using Arbiter.CommandQuery;
using Arbiter.CommandQuery.Endpoints;
using Arbiter.Dispatcher.Server;
using Arbiter.OpenTelemetry.Server;

using EstimatorX.Data;
using EstimatorX.Extensions;
using EstimatorX.Web.Components;
using EstimatorX.Web.Endpoints;
using EstimatorX.Web.Security;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;

namespace EstimatorX.Web;

public static class Program
{
    public static int Main(string[] args)
    {
        // azure app service linux doesn't set default culture
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // remove for security reasons
            builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

            ConfigureLogging(builder);
            ConfigureServices(builder);

            var app = builder.Build();
            ConfigureMiddleware(app);

            app.Run();

            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Host terminated unexpectedly: {ex.Message}");
            Console.Error.WriteLine(ex.ToString());
            return 1;
        }
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        // use shared OpenTelemetry configuration for both metrics and tracing, so we get correlation between them in Azure Monitor
        builder
            .AddOpenTelemetry(
                configureOptions: options =>
                {
                    options.BuildVersion = ThisAssembly.FileVersion;
                    options.FilteredSegments.Add("/livez");
                    options.FilteredSegments.Add("/healthz");
                }
            );
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services
            .AddRazorComponents()
            .AddInteractiveWebAssemblyComponents()
            .AddAuthenticationStateSerialization(options => options.SerializeAllClaims = true);

        services
            .AddCascadingAuthenticationState()
            .AddAuthorization();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/denied";

                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;

                options.Cookie.Name = ".EstimatorX.Authentication";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.IsEssential = true;
            })
            .AddSocialProviders(configuration);

        // persist keys to the database so auth cookies survive container restarts
        services
            .AddDataProtection()
            .SetApplicationName("EstimatorX")
            .PersistKeysToDbContext<EstimatorContext>();

        services
            .AddAntiforgery();

        services
            .AddHttpContextAccessor();

        services
            .AddEndpointRoutes()
            .AddDispatcherService();

        services
            .AddEstimatorXShared()
            .AddEstimatorXClient("Server");

        builder
            .AddEstimatorXCore()
            .AddEstimatorXWeb();

        services
            .ConfigureHttpJsonOptions(options => options.SerializerOptions.AddDomainOptions())
            .AddSingleton(sp => sp.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions);

        services
            .AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

        services
            .AddEnvironmentOptions(options =>
                options.EnvironmentName = builder.Environment.EnvironmentName
            );

        // needed do to load balancer and reverse proxy in front of the app
        services
            .Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
                options.AllowedHosts.Clear();
            });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        // Needed due to load balancer and reverse proxy in front of the app
        app.UseForwardedHeaders();

        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseResponseCompression();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

        app.UseHttpsRedirection();

        app.UseRequestLogging(config =>
        {
            config.IncludeRequestBody = true;
            config.IgnorePath("/_framework/**");
            config.IgnorePath("/_content/**");
            config.IgnorePath("/livez");
            config.IgnorePath("/healthz");
        });

        app.UseAuthentication();
        app.UseAuthorization();

        // logs claims for authenticated requests
        app.UseClaimLogging();

        app.UseAntiforgery();

        app.MapStaticAssets();

        app.MapRazorComponents<App>()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client.Routes).Assembly);

        app.MapAccountRoutes();
        app.MapEndpointRoutes().RequireAuthorization();
        app.MapDispatcherService().RequireAuthorization();

        HealthCheckOptions options = new() { Predicate = _ => false };

        app.MapHealthChecks("/livez", options).AllowAnonymous();
        app.MapHealthChecks("/healthz").AllowAnonymous();
    }
}
