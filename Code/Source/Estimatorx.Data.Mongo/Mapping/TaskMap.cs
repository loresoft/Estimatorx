using System;
using Estimatorx.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class TaskMap : BsonClassMap<Task>
    {
        public TaskMap()
        {
            MapIdProperty(c => c.Id)
                .SetRepresentation(BsonType.String)
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.Name)
                .SetElementName("nm")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.VerySimple)
                .SetElementName("vs")
                .SetIgnoreIfNull(true);
            MapProperty(c => c.Simple)
                .SetElementName("sm")
                .SetIgnoreIfNull(true);
            MapProperty(c => c.Medium)
                .SetElementName("md")
                .SetIgnoreIfNull(true);
            MapProperty(c => c.Complex)
                .SetElementName("cm")
                .SetIgnoreIfNull(true);
            MapProperty(c => c.VeryComplex)
                .SetElementName("vc")
                .SetIgnoreIfNull(true);


            MapProperty(c => c.TotalTasks)
                .SetElementName("tt")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.TotalHours)
                .SetElementName("th")
                .SetIgnoreIfDefault(true);

            MapProperty(c => c.FactorId)
                .SetElementName("f_")
                .SetRepresentation(BsonType.String)
                .SetIgnoreIfNull(true);

        }
    }
}