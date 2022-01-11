using System;

using MongoDB.Bson.Serialization;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class LogErrorMap : BsonClassMap<LogError>
    {
        public LogErrorMap()
        {
            MapProperty(c => c.Message)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.BaseMessage)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Text)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Type)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Source)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.MethodName)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.ModuleName)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.ModuleVersion)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.ErrorCode)
                .SetIgnoreIfNull(true);

            SetIgnoreExtraElements(true);
        }

    }
}