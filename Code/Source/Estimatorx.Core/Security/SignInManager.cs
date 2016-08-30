using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NLog.Fluent;

namespace Estimatorx.Core.Security
{
    public class SignInManager : Microsoft.AspNet.Identity.Owin.SignInManager<User, string>
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public SignInManager(UserManager<User, string> userManager, IAuthenticationManager authenticationManager) 
            : base(userManager, authenticationManager)
        {
        }

        public override System.Threading.Tasks.Task SignInAsync(User user, bool isPersistent, bool rememberBrowser)
        {
            _logger.Info()
                .Message("Sign in user '{0}'", user.Email)
                .Property("User", user.Email)
                .Write();

            return base.SignInAsync(user, isPersistent, rememberBrowser);
        }

        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            _logger.Debug()
                .Message("Create user identity for '{0}'", user.Email)
                .Write();

            var claimIdenity = await base.CreateUserIdentityAsync(user);

            var displayClaim = new System.Security.Claims.Claim(Claim.DisplayName, user.Name ?? user.UserName);
            claimIdenity.AddClaim(displayClaim);

            if (user.Organizations == null)
                return claimIdenity;

            foreach (var claim in user.Organizations)
            {
                var orgClaim = new System.Security.Claims.Claim(Claim.Organization, claim);
                claimIdenity.AddClaim(orgClaim);
            }

            return claimIdenity;
        }
    }
}