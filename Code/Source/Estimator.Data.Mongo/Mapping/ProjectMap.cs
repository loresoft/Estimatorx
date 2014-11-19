using System;
using Estimator.Core;
using MongoDB.Bson.Serialization;

namespace Estimator.Data.Mongo.Mapping
{
    public class ProjectMap : BsonClassMap<Project>
    {
        public ProjectMap()
        {
            AutoMap();
        }
    }
}