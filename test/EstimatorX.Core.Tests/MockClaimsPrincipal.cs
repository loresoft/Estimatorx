using System.Security.Claims;

using Arbiter.CommandQuery.Extensions;
using Arbiter.CommandQuery.Services;

namespace EstimatorX.Core.Tests;

public static class MockClaimsPrincipal
{
    public const long DefaultUserId = 900000000000000001;

    public static ClaimsPrincipal Default { get; } = CreatePrincipal(
            email: "tester@mailinator.com",
            name: "Test User",
            userId: DefaultUserId
        );

    public static ClaimsPrincipal CreatePrincipal(
        string email,
        string name,
        long? userId = null,
        IEnumerable<string>? roles = null)
    {
        var identity = new ClaimsIdentity(
            authenticationType: "Test.Application",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role
        );

        identity.AddClaim(ClaimTypes.Name, email);
        identity.AddClaim(ClaimTypes.Email, email);
        identity.AddClaim(ClaimNames.DisplayName, name);

        if (userId.HasValue)
            identity.AddClaim(ClaimNames.UserId, userId.Value);

        if (roles == null)
            return new ClaimsPrincipal(identity);

        foreach (var role in roles)
            identity.AddClaim(ClaimTypes.Role, role);

        return new ClaimsPrincipal(identity);
    }
}
