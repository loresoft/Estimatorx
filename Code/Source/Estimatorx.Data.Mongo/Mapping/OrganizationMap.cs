using System;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class OrganizationMap : BsonClassMap<Organization>
    {
        public OrganizationMap()
        {
            MapIdProperty(c => c.Id)
                .SetRepresentation(BsonType.String)
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.Name)
                .SetElementName("nm")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Description)
                .SetElementName("ds")
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

            MapProperty(c => c.Owners)
                .SetElementName("_o")
                .SetShouldSerializeMethod(ShouldSerializeOwners);
        }

        private static bool ShouldSerializeOwners(object value)
        {
            var user = value as Organization;
            if (user == null)
                return false;

            var list = user.Owners;
            return list != null && list.Count > 0;
        }

    }
}