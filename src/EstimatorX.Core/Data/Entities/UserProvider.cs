using Arbiter.CommandQuery.Definitions;

namespace EstimatorX.Data.Entities;

public class UserProvider : IHaveIdentifier<long>, ITrackCreated, ITrackUpdated
{
    public long Id { get; set; }

    public string Provider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public long UserId { get; set; }


    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;

    public string? UpdatedBy { get; set; }


    public virtual User User { get; set; } = null!;
}
