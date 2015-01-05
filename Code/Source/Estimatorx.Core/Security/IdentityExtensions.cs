using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Core.Security
{
    public static class IdentityExtensions
    {
        public static string GetDisplayName(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");
            
            var ci = identity as ClaimsIdentity;
            if (ci != null)
                return ci.FindFirstValue(Estimatorx.Core.Security.Claim.DisplayName);

            return null;
        }
        
        public static IEnumerable<string> GetOrganizations(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");

            var ci = identity as ClaimsIdentity;
            if (ci != null)
                return ci.FindValues(Estimatorx.Core.Security.Claim.Organization);

            return null;
        }

        public static IEnumerable<string> FindValues(this ClaimsIdentity identity, string claimType)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");

            var claims = identity
                .FindAll(claimType)
                .Select(c => c.Value);

            return claims;
        }

        public static bool HasOrganizationAccess(this IIdentity identity, string organizationId)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");
            if (organizationId == null)
                throw new ArgumentNullException("organizationId");

            var userId = identity.GetUserId();
            if (organizationId == userId)
                return true;

            return identity
                .GetOrganizations()
                .Contains(organizationId);
        }
    }
}