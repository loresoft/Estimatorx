using System;

using Estimatorx.Data.Mongo.Security;

using MongoDB.Bson.Serialization;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class LoginMap : BsonClassMap<Login>
    {
        public LoginMap()
        {
            MapProperty(c => c.LoginProvider)
                .SetElementName("p")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.ProviderKey)
                .SetElementName("k")
                .SetIgnoreIfNull(true);
        }
    }
}