using Arbiter.CommandQuery.Definitions;

namespace EstimatorX.Data.Entities;

public class UserLogin : IHaveIdentifier<long>, ITrackCreated, ITrackUpdated
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }


    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;

    public string? UpdatedBy { get; set; }


    public virtual User User { get; set; } = null!;
}
