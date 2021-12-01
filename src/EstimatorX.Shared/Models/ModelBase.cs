
using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class ModelBase : IHaveIdentifier<string>, ITrackCreated, ITrackUpdated
{
    public string Id { get; set; }

    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;

    public string UpdatedBy { get; set; }


    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Created, CreatedBy, Updated, UpdatedBy);
    }
}
