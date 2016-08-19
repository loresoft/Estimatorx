using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Estimatorx.Core;
using Estimatorx.Data.Mongo;
using Estimatorx.Web;
using KickStart;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Owin.Security.Providers.GitHub;

[assembly: OwinStartup(typeof(Startup))]

namespace Estimatorx.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;

            Kick.Start(c => c
                .IncludeAssemblyFor<Startup>()
                .IncludeAssemblyFor<Project>()
                .IncludeAssemblyFor<ProjectRepository>()
                .UseNLog()
                .UseAutofac(a => a
                    .Builder(b =>
                    {
                        b.RegisterControllers(typeof(Startup).Assembly);
                        b.RegisterApiControllers(typeof(Startup).Assembly);
                    })
                    .Container(r =>
                    {
                        DependencyResolver.SetResolver(new AutofacDependencyResolver(r));
                        config.DependencyResolver = new AutofacWebApiDependencyResolver(r);
                    })
                )
                .UseMongoDB()
                .UseStartupTask()
            );

            // security
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout"),
                CookieName = ".Estimatorx.Authentication",
                Provider = new CookieAuthenticationProvider()
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // social logins
            if (!string.IsNullOrEmpty(EstimatorxSettings.MicrosoftClientId))
                app.UseMicrosoftAccountAuthentication(
                    clientId: EstimatorxSettings.MicrosoftClientId,
                    clientSecret: EstimatorxSettings.MicrosoftClientSecret);

            if (!string.IsNullOrEmpty(EstimatorxSettings.TwitterConsumerKey))
                app.UseTwitterAuthentication(
                    consumerKey: EstimatorxSettings.TwitterConsumerKey,
                    consumerSecret: EstimatorxSettings.TwitterConsumerSecret);

            if (!string.IsNullOrEmpty(EstimatorxSettings.FacebookApplicationId))
                app.UseFacebookAuthentication(
                    appId: EstimatorxSettings.FacebookApplicationId,
                    appSecret: EstimatorxSettings.FacebookApplicationSecret);

            if (!string.IsNullOrEmpty(EstimatorxSettings.GoogleClientId))
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
                {
                    ClientId = EstimatorxSettings.GoogleClientId,
                    ClientSecret = EstimatorxSettings.GoogleClientSecret
                });

            if (!string.IsNullOrEmpty(EstimatorxSettings.GitHubClientId))
                app.UseGitHubAuthentication(new GitHubAuthenticationOptions
                {
                    ClientId = EstimatorxSettings.GitHubClientId,
                    ClientSecret = EstimatorxSettings.GitHubClientSecret,
                    Scope = { "user:email" }
                });
        }

    }
}