using EstimatorX.Security;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace EstimatorX.Web.Security;

public static class AuthenticationExtensions
{
    public static AuthenticationBuilder AddSocialProviders(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        AddGoogle(builder, configuration);
        AddMicrosoft(builder, configuration);
        AddFacebook(builder, configuration);
        AddGitHub(builder, configuration);

        return builder;
    }

    private static void AddFacebook(AuthenticationBuilder builder, IConfiguration configuration)
    {
        var appId = configuration[AuthenticationProviders.Facebook.AppId];
        var appSecret = configuration[AuthenticationProviders.Facebook.AppSecret];

        if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appSecret))
            return;

        builder.AddFacebook(options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.AppId = appId;
            options.AppSecret = appSecret;
            ConfigureEvents(options);
        });
    }

    private static void AddGitHub(AuthenticationBuilder builder, IConfiguration configuration)
    {
        var clientId = configuration[AuthenticationProviders.GitHub.ClientId];
        var clientSecret = configuration[AuthenticationProviders.GitHub.ClientSecret];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            return;

        builder.AddGitHub(options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.Scope.Add("user:email");
            ConfigureEvents(options);
        });
    }

    private static void AddMicrosoft(AuthenticationBuilder builder, IConfiguration configuration)
    {
        var clientId = configuration[AuthenticationProviders.Microsoft.ClientId];
        var clientSecret = configuration[AuthenticationProviders.Microsoft.ClientSecret];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            return;

        builder.AddMicrosoftAccount(options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            ConfigureEvents(options);
        });
    }

    private static void AddGoogle(AuthenticationBuilder builder, IConfiguration configuration)
    {
        var clientId = configuration[AuthenticationProviders.Google.ClientId];
        var clientSecret = configuration[AuthenticationProviders.Google.ClientSecret];

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            return;

        builder.AddGoogle(options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            ConfigureEvents(options);
        });
    }

    private static void ConfigureEvents(OAuthOptions options)
    {
        options.Events.OnCreatingTicket = ExternalLoginEvents.OnCreatingTicket;
        options.Events.OnRemoteFailure = ExternalLoginEvents.OnRemoteFailure;
    }
}
