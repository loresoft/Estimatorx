using System;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class InviteMap : BsonClassMap<Invite>
    {
        public InviteMap()
        {
            MapIdProperty(c => c.Id)
                .SetRepresentation(BsonType.String)
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.Email)
                .SetElementName("em")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.OrganizationId)
                .SetElementName("o_")
                .SetRepresentation(BsonType.String)
                .SetIgnoreIfNull(true);

            MapProperty(c => c.SecurityKey)
                .SetElementName("sk")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Created)
                .SetElementName("cd")
                .SetSerializationOptions(new DateTimeSerializationOptions { Kind = DateTimeKind.Local });
            MapProperty(c => c.Creator)
                .SetElementName("cu")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Updated)
                .SetElementName("ud")
                .SetSerializationOptions(new DateTimeSerializationOptions { Kind = DateTimeKind.Local });
            MapProperty(c => c.Updater)
                .SetElementName("uu")
                .SetIgnoreIfNull(true);

        }
    }
}