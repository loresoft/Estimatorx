using System;
using System.Collections.Generic;
using Estimatorx.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;

namespace Estimatorx.Data.Mongo.Mapping
{
    public class ProjectMap : BsonClassMap<Project>
    {
        public ProjectMap()
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

            MapProperty(c => c.HoursPerWeek)
                .SetElementName("hw")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.ContingencyRate)
                .SetElementName("cr")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.TotalTasks)
                .SetElementName("tt")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.TotalHours)
                .SetElementName("th")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.TotalWeeks)
                .SetElementName("tw")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.ContingencyHours)
                .SetElementName("ch")
                .SetIgnoreIfDefault(true);
            MapProperty(c => c.ContingencyWeeks)
                .SetElementName("cw")
                .SetIgnoreIfDefault(true);

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


            MapProperty(c => c.Assumptions)
                .SetElementName("_a")
                .SetShouldSerializeMethod(ShouldSerializeAssumptions);
            MapProperty(c => c.Factors)
                .SetElementName("_f")
                .SetShouldSerializeMethod(ShouldSerializeFactors);
            MapProperty(c => c.Sections)
                .SetElementName("_s")
                .SetShouldSerializeMethod(ShouldSerializeSections);

        }

        private static bool ShouldSerializeSections(object value)
        {
            var project = value as Project;
            if (project == null)
                return false;
            
            var list = project.Sections;
            return list != null && list.Count > 0;
        }

        private static bool ShouldSerializeFactors(object value)
        {
            var project = value as Project;
            if (project == null)
                return false;

            var list = project.Factors;
            return list != null && list.Count > 0;
        }

        private static bool ShouldSerializeAssumptions(object value)
        {
            var project = value as Project;
            if (project == null)
                return false;
            
            var list = project.Assumptions;
            return list != null && list.Count > 0;
        }
    }
}