using System;
using Estimatorx.Core;
using MongoDB.Bson.Serialization;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class SectionMap : BsonClassMap<Section>
    {
        public SectionMap()
        {
            AutoMap();
        }
    }
}