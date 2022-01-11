using System;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class FactorMap : BsonClassMap<Factor>
    {
        public FactorMap()
        {
            MapIdProperty(c => c.Id)
                .SetSerializer(new StringSerializer(BsonType.ObjectId))
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.Name)
                .SetElementName("nm")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.VerySimple)
                .SetElementName("vs")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.Simple)
                .SetElementName("sm")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.Medium)
                .SetElementName("md")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.Complex)
                .SetElementName("cm")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.VeryComplex)
                .SetElementName("vc")
                .SetIgnoreIfDefault(true);
        }
    }
}