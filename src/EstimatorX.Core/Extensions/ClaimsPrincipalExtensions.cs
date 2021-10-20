using System;
using System.Security.Claims;
using System.Security.Principal;

namespace EstimatorX.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetDisplayName(this IPrincipal principal)
        {
            var claimPrincipal = principal as ClaimsPrincipal;
            return claimPrincipal?.FindFirstValue(Entities.User.ClaimTypes.DisplayName) ?? principal.Identity?.Name;
        }


        public static string GetUserId(this IPrincipal principal)
        {
            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claimPrincipal = principal as ClaimsPrincipal;
            return claimPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
