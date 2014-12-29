using System;
using Estimatorx.Core.Security;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class UserMap : BsonClassMap<User>
    {
        public UserMap()
        {
            MapIdProperty(c => c.Id)
                .SetRepresentation(BsonType.String)
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.UserName)
                .SetElementName("un")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Email)
                .SetElementName("em")
                .SetIgnoreIfNull(true);
            MapProperty(c => c.EmailConfirmed)
                .SetElementName("ec")
                .SetIgnoreIfDefault(true);

            MapProperty(c => c.PasswordHash)
                .SetElementName("pw")
                .SetIgnoreIfNull(true);
            MapProperty(c => c.SecurityStamp)
                .SetElementName("ss")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.PhoneNumber)
                .SetElementName("ph")
                .SetIgnoreIfNull(true);
            MapProperty(c => c.PhoneNumberConfirmed)
                .SetElementName("pc")
                .SetIgnoreIfDefault(true);

            MapProperty(c => c.TwoFactorEnabled)
                .SetElementName("tf")
                .SetIgnoreIfDefault(true);

            MapProperty(c => c.LockoutEndDateUtc)
                .SetElementName("ld")
                .SetIgnoreIfNull(true);
            MapProperty(c => c.LockoutEnabled)
                .SetElementName("le")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.AccessFailedCount)
                .SetElementName("af")
                .SetIgnoreIfDefault(true);

            MapProperty(c => c.Roles)
                .SetElementName("_r")
                .SetShouldSerializeMethod(ShouldSerializeRoles);
            MapProperty(c => c.Claims)
                .SetElementName("_c")
                .SetShouldSerializeMethod(ShouldSerializeClaims);
            MapProperty(c => c.Logins)
                .SetElementName("_l")
                .SetShouldSerializeMethod(ShouldSerializeLogins);
        }

        private static bool ShouldSerializeRoles(object value)
        {
            var user = value as User;
            if (user == null)
                return false;

            var list = user.Roles;
            return list != null && list.Count > 0;
        }

        private static bool ShouldSerializeClaims(object value)
        {
            var user = value as User;
            if (user == null)
                return false;

            var list = user.Claims;
            return list != null && list.Count > 0;
        }

        private static bool ShouldSerializeLogins(object value)
        {
            var user = value as User;
            if (user == null)
                return false;

            var list = user.Logins;
            return list != null && list.Count > 0;
        }

    }
}