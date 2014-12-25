using System;
using Estimatorx.Core;
using MongoDB.Bson.Serialization;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class FactorMap : BsonClassMap<Factor>
    {
        public FactorMap()
        {
            AutoMap();
        }
    }
}