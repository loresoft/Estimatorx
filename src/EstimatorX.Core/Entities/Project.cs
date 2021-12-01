using Cosmos.Abstracts;

using EstimatorX.Shared.Definitions;

namespace EstimatorX.Core.Entities;

public class Project : EntityBase, IHaveOrganization
{
    public string Name { get; set; }

    public string Description { get; set; }

    [PartitionKey]
    public string OrganizationId { get; set; }

    public string SecurityKey { get; set; }
}
