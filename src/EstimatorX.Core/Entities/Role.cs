using Cosmos.Identity;

using MediatR.CommandQuery.Definitions;

namespace EstimatorX.Core.Entities
{
    public class Role : IdentityRole, IHaveIdentifier<string>, ITrackCreated, ITrackUpdated
    {
        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}