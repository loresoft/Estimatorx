using System;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace Estimatorx.Data.Mongo
{
    /// <summary>
    /// An estimate based on number of tasks for a project <see cref="Factor"/>.
    /// </summary>
    public class Task
    {
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
        /// Gets or sets the name of the estimate .
        /// </summary>
        /// <value>
        /// The name of the estimate.
        /// </value>
        [BsonElement("nm")]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the number of very simple tasks.
        /// </summary>
        /// <value>
        /// The number of very simple tasks.
        /// </value>
        [BsonElement("vs")]
        public byte? VerySimple { get; set; }

        /// <summary>
        /// Gets or sets the number of simple tasks.
        /// </summary>
        /// <value>
        /// The number of simple tasks.
        /// </value>
        [BsonElement("sm")]
        public byte? Simple { get; set; }

        /// <summary>
        /// Gets or sets the number of medium tasks.
        /// </summary>
        /// <value>
        /// The number of medium tasks.
        /// </value>
        [BsonElement("md")]
        public byte? Medium { get; set; }

        /// <summary>
        /// Gets or sets the number of complex tasks.
        /// </summary>
        /// <value>
        /// The number of complex tasks.
        /// </value>
        [BsonElement("cm")]
        public byte? Complex { get; set; }

        /// <summary>
        /// Gets or sets the number of very complex tasks.
        /// </summary>
        /// <value>
        /// The number of very complex tasks.
        /// </value>
        [BsonElement("vc")]
        public byte? VeryComplex { get; set; }


        /// <summary>
        /// Gets or sets the total number of tasks for this estimate.
        /// </summary>
        /// <value>
        /// The total number of tasks for this estimate.
        /// </value>
        [BsonElement("tt")]
        public int TotalTasks { get; set; }

        /// <summary>
        /// Gets or sets the total number hours for this estimate based on the <see cref="Factor"/>.
        /// </summary>
        /// <value>
        /// The total number hours for this estimate based on the <see cref="Factor"/>.
        /// </value>
        [BsonElement("th")]
        public int TotalHours { get; set; }


        /// <summary>
        /// Gets or sets the factor identifier for this estimate.
        /// </summary>
        /// <value>
        /// The factor identifier for this estimate.
        /// </value>
        [BsonElement("f_")]
        public string FactorId { get; set; }
    }
}
