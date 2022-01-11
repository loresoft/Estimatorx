using System;
using System.Collections.Generic;
using System.ComponentModel;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace Estimatorx.Data.Mongo
{
    /// <summary>
    /// A group of estimates for a section of the application being estimated.
    /// </summary>
    public class Section
    {
        public Section()
        {
            Tasks = new List<Task>();
        }

        /// <summary>
        /// Gets or sets the identifier for the model.
        /// </summary>
        /// <value>
        /// The identifier for the model.
        /// </value>
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        [BsonElement("nm")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the total number of tasks for this section.
        /// </summary>
        /// <value>
        /// The total number of tasks for this section.
        /// </value>
        [DefaultValue(0)]
        [BsonElement("tt")]
        public int TotalTasks { get; set; }

        /// <summary>
        /// Gets or sets the total number hours for this section.
        /// </summary>
        /// <value>
        /// The total number hours for this section.
        /// </value>
        [DefaultValue(0)]
        [BsonElement("th")]
        public int TotalHours { get; set; }

        /// <summary>
        /// Gets or sets the total number of weeks for this section based on the number of hours per week for the project.
        /// </summary>
        /// <value>
        /// The total number of weeks for this section based on the number of hours per week for the project.
        /// </value>
        /// <seealso cref="P:Project.HoursPerWeek"/>
        [DefaultValue(0)]
        [BsonElement("tw")]
        public double TotalWeeks { get; set; }


        /// <summary>
        /// Gets or sets the estimates for this section.
        /// </summary>
        /// <value>
        /// The estimates for this section.
        /// </value>
        [BsonElement("_t")]
        public List<Task> Tasks { get; set; }
    }
}
