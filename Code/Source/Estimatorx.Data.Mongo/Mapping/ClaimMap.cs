using System;
using Estimatorx.Core.Security;
using MongoDB.Bson.Serialization;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class ClaimMap : BsonClassMap<Claim>
    {
        public ClaimMap()
        {
            MapProperty(c => c.Type)
                .SetElementName("t")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Type)
                .SetElementName("v")
                .SetIgnoreIfNull(true);
        }
    }
}