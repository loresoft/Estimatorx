using System;
using System.Security.Claims;
using System.Security.Principal;

namespace EstimatorX.Shared.Extensions;

public static class PrincipalExtensions
{
    private const string ObjectIdenttifier = "oid";
    private const string NameClaim = "name";
    private const string EmailClaim = "email";
    private const string EmailsClaim = "emails";
    private const string ProviderClaim = "idp";
    private const string IdentityClaim = "http://schemas.microsoft.com/identity/claims/identityprovider";

    public static string GetEmail(this IPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentNullException(nameof(principal));

        var claimPrincipal = principal as ClaimsPrincipal;
        var claim = claimPrincipal?.FindFirst(ClaimTypes.Email)
            ?? claimPrincipal?.FindFirst(EmailClaim)
            ?? claimPrincipal?.FindFirst(EmailsClaim);

        return claim?.Value ?? string.Empty;
    }


    public static string GetUserId(this IPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentNullException(nameof(principal));

        var claimPrincipal = principal as ClaimsPrincipal;
        var claim = claimPrincipal?.FindFirst(ClaimTypes.NameIdentifier)
            ?? claimPrincipal?.FindFirst(ObjectIdenttifier);

        return claim?.Value ?? string.Empty;
    }


    public static string GetName(this IPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentNullException(nameof(principal));

        var claimPrincipal = principal as ClaimsPrincipal;
        var claim = claimPrincipal?.FindFirst(ClaimTypes.Name)
            ?? claimPrincipal?.FindFirst(NameClaim);

        return claim?.Value ?? string.Empty;
    }

    public static string GetProvider(this IPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentNullException(nameof(principal));

        var claimPrincipal = principal as ClaimsPrincipal;
        var claim = claimPrincipal?.FindFirst(ProviderClaim)
            ?? claimPrincipal?.FindFirst(IdentityClaim);

        return claim?.Value ?? string.Empty;
    }

}
