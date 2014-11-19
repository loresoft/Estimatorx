using System;
using Estimator.Core;
using MongoDB.Bson.Serialization;

namespace Estimator.Data.Mongo.Mapping
{
    public class SectionMap : BsonClassMap<Section>
    {
        public SectionMap()
        {
            AutoMap();
        }
    }
}