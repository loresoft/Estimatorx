using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using LoreSoft.Blazor.Controls;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster.Core.Models;

namespace EstimatorX.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var services = builder.Services;

            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            services
                .AddHttpClient("default", client =>
                {
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                })
                .AddHttpMessageHandler<ProgressBarHandler>();

            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("default"));

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
                .AddSingleton<OrganizationStore>()
                .AddSingleton<UserStore>()
                .AddSingleton<ProjectStore>()
                .AddSingleton<TemplateStore>();

            services
                .AddScoped<OrganizationRepository>()
                .AddScoped<ProjectRepository>()
                .AddScoped<TemplateRepository>();

            services
                .AddScoped<NotificationService>();

            await builder.Build().RunAsync();
        }
    }
}
