using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cosmos.Identity;
using EstimatorX.Core.Entities;
using EstimatorX.Core.Options;
using EstimatorX.Service.Security;
using KickStart;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SendGrid.Extensions.DependencyInjection;

namespace EstimatorX.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .KickStart(c => c
                    .IncludeAssemblyFor<ConfigurationDependencyRegistration>()
                    .IncludeAssemblyFor<Startup>()
                    .Data(ConfigurationDependencyRegistration.ConfigurationKey, Configuration)
                    .Data("hostProcess", "web")
                    .UseStartupTask()
                );

            services
                .AddIdentity<User, Role>()
                .AddCosmosStores()
                .AddDefaultTokenProviders()
                .AddPasswordlessLoginProvider();


            services
                .AddControllers();

            services
                .AddRazorPages()
                .AddRazorPagesOptions(options => options.Conventions.AuthorizePage("/_Host"));

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EstimatorX", Version = "v1" });
                });


            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    options.CheckConsentNeeded = context => false;
                    options.MinimumSameSitePolicy = SameSiteMode.Lax;
                });

            services
                .Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                });

            services
                .ConfigureApplicationCookie(options =>
                {
                    var originalEvents = options.Events;

                    options.Cookie.HttpOnly = true;
                    options.Cookie.Name = ".EstimatorX.Authentication";
                    options.SlidingExpiration = true;
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            services
                .Configure<PasswordlessLoginProviderOptions>(options =>
                {
                    options.TokenLifespan = TimeSpan.FromMinutes(15);
                });

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

            app.UseExceptionHandler("/Error");

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
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
