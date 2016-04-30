using System;
using System.Collections.Generic;
using Estimatorx.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class SectionMap : BsonClassMap<Section>
    {
        public SectionMap()
        {
            MapIdProperty(c => c.Id)
                .SetSerializer(new StringSerializer(BsonType.ObjectId))
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.Name)
                .SetElementName("nm")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.TotalTasks)
                .SetElementName("tt")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.TotalHours)
                .SetElementName("th")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.TotalWeeks)
                .SetElementName("tw")
                .SetIgnoreIfDefault(true);

            MapProperty(c => c.Tasks)
                .SetElementName("_t")
                .SetShouldSerializeMethod(ShouldSerializeTasks);
        }

        private static bool ShouldSerializeTasks(object value)
        {
            var section = value as Section;
            if (section == null)
                return false;

            var list = section.Tasks;
            return list != null && list.Count > 0;
        }

    }
}