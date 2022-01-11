using System;

using Estimatorx.Data.Mongo.Security;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class RoleMap : BsonClassMap<Role>
    {
        public RoleMap()
        {
            MapIdProperty(c => c.Id)
                .SetSerializer(new StringSerializer(BsonType.ObjectId))
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.Name)
                .SetElementName("nm")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Created)
                .SetElementName("cd")
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Local));

            MapProperty(c => c.Updated)
                .SetElementName("ud")
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Local));
        }
    }
}