using Arbiter.CommandQuery.Definitions;

namespace EstimatorX.Data.Entities;

public class UserOrganization : IHaveIdentifier<long>, ITrackCreated, ITrackUpdated
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long OrganizationId { get; set; }

    public bool IsOwner { get; set; }


    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;

    public string? UpdatedBy { get; set; }


    public virtual Organization Organization { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
