using System;
using Estimator.Core;
using MongoDB.Bson.Serialization;

namespace Estimator.Data.Mongo.Mapping
{
    public class AssumptionMap : BsonClassMap<Assumption>
    {
        public AssumptionMap()
        {
            AutoMap();
        }
    }
}