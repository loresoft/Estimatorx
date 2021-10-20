using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Client.Services
{
    public class UserAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly UserStore _userStore;
        private readonly ILogger _logger;

        public UserAuthenticationStateProvider(HttpClient httpClient, UserStore userStore, ILogger<UserAuthenticationStateProvider> logger)
        {
            _httpClient = httpClient;
            _userStore = userStore;
            _logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userData = await _httpClient.GetFromJsonAsync<UserModel>("/api/user/me");
                if (userData == null || !userData.IsAuthenticated)
                    return NotAuthorized();

                _userStore.Set(userData);

                _logger.LogInformation("User {userName} authenticated.", userData.UserName);

                var identity = new ClaimsIdentity("Server Authentication");
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userData.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, userData.DisplayName));
                identity.AddClaim(new Claim(ClaimTypes.Email, userData.Email));

                if (userData.Roles.Count > 0)
                    identity.AddClaims(userData.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var claimsPrincipal = new ClaimsPrincipal(identity);

                return new AuthenticationState(claimsPrincipal);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error loading user authentication: {message}", ex.Message);
                return NotAuthorized();
            }
        }

        private AuthenticationState NotAuthorized()
        {
            _userStore.Clear();

            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);

            return new AuthenticationState(principal);
        }

    }
}
