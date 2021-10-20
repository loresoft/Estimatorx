using System;
using MediatR.CommandQuery.Definitions;

namespace EstimatorX.Shared.Models
{
    public class ModelBase : IHaveIdentifier<string>, ITrackCreated, ITrackUpdated
    {
        public string Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset Updated { get; set; }

        public string UpdatedBy { get; set; }
    }

}