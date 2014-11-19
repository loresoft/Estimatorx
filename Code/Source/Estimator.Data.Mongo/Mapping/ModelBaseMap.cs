using System;
using Estimator.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;

namespace Estimator.Data.Mongo.Mapping
{
    public class ModelBaseMap : BsonClassMap<ModelBase>
    {
        public ModelBaseMap()
        {
            AutoMap();

            var idMember = GetMemberMap(p => p.Id)
                .SetRepresentation(BsonType.String)
                .SetIdGenerator(GuidGenerator.Instance);

            SetIdMember(idMember);

            GetMemberMap(c => c.SysCreateDate)
                .SetSerializationOptions(new DateTimeSerializationOptions { Kind = DateTimeKind.Local });

            GetMemberMap(c => c.SysUpdateDate)
                .SetSerializationOptions(new DateTimeSerializationOptions { Kind = DateTimeKind.Local });
        }
    }
}
