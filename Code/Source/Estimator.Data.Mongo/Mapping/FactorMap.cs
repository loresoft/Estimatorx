using System;
using Estimator.Core;
using MongoDB.Bson.Serialization;

namespace Estimator.Data.Mongo.Mapping
{
    public class FactorMap : BsonClassMap<Factor>
    {
        public FactorMap()
        {
            AutoMap();
        }
    }
}