using Cosmos.Abstracts;

using EstimatorX.Shared.Definitions;

namespace EstimatorX.Core.Entities;

public class EntityBase : CosmosEntity, IHaveIdentifier<string>, ITrackCreated, ITrackUpdated
{
    public string CreatedBy { get; set; }

    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string UpdatedBy { get; set; }

    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;
}
