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

            var loreSoftClaim = new System.Security.Claims.Claim("Organization", "LoreSoft");
            claimIdenity.AddClaim(loreSoftClaim);
            var novusClaim = new System.Security.Claims.Claim("Organization", "Novus");
            claimIdenity.AddClaim(novusClaim);

            return claimIdenity;
        }
    }
}