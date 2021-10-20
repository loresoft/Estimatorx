using Cosmos.Abstracts;

using MediatR.CommandQuery.Definitions;

namespace EstimatorX.Core.Entities
{
    public class EntityBase : CosmosEntity, IHaveIdentifier<string>, ITrackCreated, ITrackUpdated
    {
        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}