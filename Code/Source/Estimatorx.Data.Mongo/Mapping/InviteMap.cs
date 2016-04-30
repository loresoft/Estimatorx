using System;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class InviteMap : BsonClassMap<Invite>
    {
        public InviteMap()
        {
            MapIdProperty(c => c.Id)
                .SetSerializer(new StringSerializer(BsonType.ObjectId))
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.Email)
                .SetElementName("em")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.OrganizationId)
                .SetElementName("o_")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.SecurityKey)
                .SetElementName("sk")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Created)
                .SetElementName("cd")
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Local));
            MapProperty(c => c.Creator)
                .SetElementName("cu")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Updated)
                .SetElementName("ud")
                .SetSerializer(new DateTimeSerializer(DateTimeKind.Local));
            MapProperty(c => c.Updater)
                .SetElementName("uu")
                .SetIgnoreIfNull(true);

        }
    }
}