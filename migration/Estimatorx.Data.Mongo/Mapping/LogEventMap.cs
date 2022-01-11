using System;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class LogEventMap : BsonClassMap<LogEvent>
    {
        public LogEventMap()
        {
            MapIdProperty(c => c.Id)
                .SetSerializer(new StringSerializer(BsonType.ObjectId))
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.Date)
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Local));

            MapProperty(c => c.Level)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Logger)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Message)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Source)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Correlation)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Exception)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Properties)
                .SetShouldSerializeMethod(ShouldSerializeProperties);

            SetIgnoreExtraElements(true);
        }

        private static bool ShouldSerializeProperties(object value)
        {
            var logEvent = value as LogEvent;
            if (logEvent == null)
                return false;

            var list = logEvent.Properties;
            return list != null && list.Count > 0;
        }

    }
}
