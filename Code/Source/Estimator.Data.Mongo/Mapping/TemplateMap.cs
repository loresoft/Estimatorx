using System;
using Estimator.Core;
using MongoDB.Bson.Serialization;

namespace Estimator.Data.Mongo.Mapping
{
    public class TemplateMap : BsonClassMap<Template>
    {
        public TemplateMap()
        {
            AutoMap();
        }
    }
}