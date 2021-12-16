using System.Net.Http.Json;
using System.Security.Claims;

using EstimatorX.Client.Stores;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace EstimatorX.Client.Services;

public class AccountClaimsFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    private readonly ILogger<AccountClaimsFactory> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly UserStore _userStore;

    public AccountClaimsFactory(IAccessTokenProviderAccessor accessor, ILogger<AccountClaimsFactory> logger, IServiceProvider serviceProvider, UserStore userStore) : base(accessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var initialUser = await base.CreateUserAsync(account, options);

        if (initialUser.Identity?.IsAuthenticated != true)
            return initialUser;


        var userIdentity = (ClaimsIdentity)initialUser.Identity;

        try
        {
            var gateway = _serviceProvider.GetRequiredService<GatewayClient>();

            var userData = await gateway.HttpClient.GetFromJsonAsync<User>("/api/user/me");
            if (userData == null)
                return initialUser;

            _userStore.Set(userData);

            _logger.LogInformation("User {userName} authenticated.", userData.Name);

            if (userData.Email.HasValue())
                userIdentity.AddClaim(new Claim(ClaimTypes.Email, userData.Email));

            if (userData.Roles.Count > 0)
                userIdentity.AddClaims(userData.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user authentication: {message}", ex.Message);
        }

        return initialUser;
    }
}
