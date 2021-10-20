using System.Collections.Generic;
using System.Linq;

using Cosmos.Identity;

using MediatR.CommandQuery.Definitions;

using Newtonsoft.Json;

namespace EstimatorX.Core.Entities
{
    public class User : IdentityUser, IHaveIdentifier<string>, ITrackCreated, ITrackUpdated
    {
        [JsonIgnore]
        public string DisplayName
        {
            get => GetClaim(ClaimTypes.DisplayName);
            set => SetClaim(ClaimTypes.DisplayName, value);
        }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public HashSet<string> Organizations { get; set; } = new HashSet<string>();

        protected void SetClaim(string type, string value)
        {
            var claim = Claims
                .Where(c => c.Type == type)
                .FirstOrDefault();

            if (claim == null)
            {
                claim = new IdentityClaim { Type = type };
                Claims.Add(claim);
            }

            claim.Value = value;
        }

        protected string GetClaim(string type)
        {
            return Claims
                .Where(c => c.Type == type)
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        public static class ClaimTypes
        {
            public const string DisplayName = "displayName";
        }

    }
}