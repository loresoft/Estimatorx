using AspNet.Security.OAuth.GitHub;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Web.Endpoints;

public static class AccountEndpoints
{
    private static readonly string[] AllowedProviders =
    [
        MicrosoftAccountDefaults.AuthenticationScheme,
        GoogleDefaults.AuthenticationScheme,
        FacebookDefaults.AuthenticationScheme,
        GitHubAuthenticationDefaults.AuthenticationScheme
    ];

    public static IEndpointRouteBuilder MapAccountRoutes(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/account");

        group.MapGet("/challenge/{provider}", Challenge);
        group.MapPost("/logout", SignOut);

        return endpoints;
    }

    private static Results<BadRequest, ChallengeHttpResult> Challenge(string provider, [FromQuery] string? returnUrl)
    {
        var scheme = AllowedProviders.FirstOrDefault(p => string.Equals(p, provider, StringComparison.OrdinalIgnoreCase));
        if (scheme is null)
            return TypedResults.BadRequest();

        var properties = GetAuthProperties(returnUrl);
        return TypedResults.Challenge(properties, [scheme]);
    }

    private static SignOutHttpResult SignOut([FromQuery] string? returnUrl)
    {
        var properties = GetAuthProperties(returnUrl);
        return TypedResults.SignOut(properties, [CookieAuthenticationDefaults.AuthenticationScheme]);
    }

    private static AuthenticationProperties GetAuthProperties(string? returnUrl)
    {
        const string pathBase = "/";

        if (string.IsNullOrEmpty(returnUrl))
            returnUrl = pathBase;
        else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
            returnUrl = new Uri(returnUrl, UriKind.Absolute).PathAndQuery;
        else if (returnUrl[0] != '/')
            returnUrl = $"{pathBase}{returnUrl}";

        return new AuthenticationProperties { RedirectUri = returnUrl, };
    }
}
