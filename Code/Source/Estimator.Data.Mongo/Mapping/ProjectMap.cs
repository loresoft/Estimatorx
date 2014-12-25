using System;
using Estimatorx.Core;
using MongoDB.Bson.Serialization;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class ProjectMap : BsonClassMap<Project>
    {
        public ProjectMap()
        {
            AutoMap();
        }
    }
}