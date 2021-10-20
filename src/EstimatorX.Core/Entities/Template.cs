using Cosmos.Abstracts;

namespace EstimatorX.Core.Entities
{
    public class Template : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [PartitionKey]
        public string OrganizationId { get; set; }
    }
}
