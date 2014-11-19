using System;
using Estimator.Core;
using MongoDB.Bson.Serialization;

namespace Estimator.Data.Mongo.Mapping
{
    public class EstimateMap : BsonClassMap<Estimate>
    {
        public EstimateMap()
        {
            AutoMap();
        }
    }
}