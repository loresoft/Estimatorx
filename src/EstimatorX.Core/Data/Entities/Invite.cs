using Arbiter.CommandQuery.Definitions;

namespace EstimatorX.Data.Entities;

public class Invite : IHaveIdentifier<long>, ITrackCreated, ITrackUpdated
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public long OrganizationId { get; set; }

    public string? SecurityCode { get; set; }

    public DateTimeOffset Expiration { get; set; } = DateTimeOffset.UtcNow.AddDays(30);

    public DateTimeOffset? LastSent { get; set; }


    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;

    public string? UpdatedBy { get; set; }


    public virtual Organization Organization { get; set; } = null!;
}
