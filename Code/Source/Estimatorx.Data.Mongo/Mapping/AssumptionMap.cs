using System;
using Estimatorx.Core;
using MongoDB.Bson.Serialization;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class AssumptionMap : BsonClassMap<Assumption>
    {
        public AssumptionMap()
        {
            AutoMap();
        }
    }
}