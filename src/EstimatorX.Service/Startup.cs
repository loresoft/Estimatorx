
using EstimatorX.Core.Options;
using EstimatorX.Service.Middleware;
using EstimatorX.Shared;

using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

using SendGrid.Extensions.DependencyInjection;

namespace EstimatorX.Service;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddCosmosRepository();

        services.AddEstimatorXCore();
        services.AddEstimatorXShared();
        services.AddEstimatorXService();

        services.AddSendGrid((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IOptions<SendGridConfiguration>>();
            options.ApiKey = configuration.Value.ApiKey ?? "***";
        });

        services.AddAutoMapper(typeof(HostingConfiguration).Assembly, typeof(AssemblyMetadata).Assembly);

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

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

        services
            .AddControllers();

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services
            .AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "EstimatorX", Version = "v1" }));


        services.Configure<ApiBehaviorOptions>(apiBehaviorOptions => apiBehaviorOptions.SuppressModelStateInvalidFilter = true);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseHsts();
        }

        app.UseMiddleware<JsonExceptionMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EstimatorX v1"));

        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}

