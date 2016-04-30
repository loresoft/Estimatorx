using System;
using System.Collections.Generic;
using Estimatorx.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class TemplateMap : BsonClassMap<Template>
    {
        public TemplateMap()
        {
            MapIdProperty(c => c.Id)
                .SetSerializer(new StringSerializer(BsonType.ObjectId))
                .SetIdGenerator(StringObjectIdGenerator.Instance);

            MapProperty(c => c.Name)
                .SetElementName("nm")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.Description)
                .SetElementName("ds")
                .SetIgnoreIfNull(true);

            MapProperty(c => c.OrganizationId)
                .SetElementName("o_")
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

            MapProperty(c => c.Factors)
                .SetElementName("_f")
                .SetShouldSerializeMethod(ShouldSerializeFactors);

        }

        private static bool ShouldSerializeFactors(object value)
        {
            var template = value as Template;
            if (template == null)
                return false;

            var list = template.Factors;
            return list != null && list.Count > 0;
        }

    }
}