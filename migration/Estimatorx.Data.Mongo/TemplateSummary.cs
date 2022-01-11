using System;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace Estimatorx.Data.Mongo
{
    public class TemplateSummary
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
        /// Gets or sets the name of the template.
        /// </summary>
        /// <value>
        /// The name of the template.
        /// </value>
        [BsonElement("nm")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for the template.
        /// </summary>
        /// <value>
        /// The description for the template.
        /// </value>
        [BsonElement("ds")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or set the organization this project belongs to.
        /// </summary>
        [BsonElement("o_")]
        public string OrganizationId { get; set; }


        /// <summary>
        /// Gets or sets the system create date.
        /// </summary>
        /// <value>
        /// The system create date.
        /// </value>
        [BsonElement("cd")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the system create user.
        /// </summary>
        /// <value>
        /// The system create user.
        /// </value>
        [BsonElement("cu")]
        public string Creator { get; set; }

        /// <summary>
        /// Gets or sets the system update date.
        /// </summary>
        /// <value>
        /// The system update date.
        /// </value>
        [BsonElement("ud")]
        public DateTime Updated { get; set; }

        /// <summary>
        /// Gets or sets the system update user.
        /// </summary>
        /// <value>
        /// The system update user.
        /// </value>
        [BsonElement("uu")]
        public string Updater { get; set; }

    }
}
