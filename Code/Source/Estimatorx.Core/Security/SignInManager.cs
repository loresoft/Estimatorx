using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Estimatorx.Core.Security
{
    public class SignInManager : Microsoft.AspNet.Identity.Owin.SignInManager<User, string>
    {
        public SignInManager(UserManager<User, string> userManager, IAuthenticationManager authenticationManager) 
            : base(userManager, authenticationManager)
        {
        }

        public override async Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
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