using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace Estimatorx.Data.Mongo
{
    /// <summary>
    /// A project estimate model
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            HoursPerWeek = 30;
            ContingencyRate = 10;

            Assumptions = new List<string>();
            Factors = new List<Factor>();
            Sections = new List<Section>();
        }


        /// <summary>
        /// Gets or sets the identifier for the model.
        /// </summary>
        /// <value>
        /// The identifier for the model.
        /// </value>
        [Required]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        [Required]
        [BsonElement("nm")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the project.
        /// </summary>
        /// <value>
        /// The description of the project.
        /// </value>
        [BsonElement("ds")]
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets the hours per week to use when calculating an estimates total number of weeks.
        /// </summary>
        /// <value>
        /// The hours per week to use when calculating an estimates total number of weeks.
        /// </value>
        [DefaultValue(0)]
        [BsonElement("hw")]
        public int HoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the contingency percentage rate for the project. Contingency rate is multiplied
        /// by the <see cref="TotalHours"/> to give the <see cref="ContingencyHours"/>.
        /// </summary>
        /// <value>
        /// The contingency percentage rate for the project.
        /// </value>
        [DefaultValue(0)]
        [BsonElement("cr")]
        public double ContingencyRate { get; set; }

        /// <summary>
        /// Gets or sets the total number tasks estimated for the project.
        /// </summary>
        /// <value>
        /// The total number tasks estimated for the project.
        /// </value>
        [DefaultValue(0)]
        [BsonElement("tt")]
        public int TotalTasks { get; set; }

        /// <summary>
        /// Gets or sets the total number hours estimated for the project.
        /// </summary>
        /// <value>
        /// The total hours number estimated for the project.
        /// </value>
        [DefaultValue(0)]
        [BsonElement("th")]
        public int TotalHours { get; set; }

        /// <summary>
        /// Gets or sets the total number weeks estimated for the project.
        /// </summary>
        /// <value>
        /// The total number weeks estimated for the project.
        /// </value>
        [DefaultValue(0)]
        [BsonElement("tw")]
        public double TotalWeeks { get; set; }

        /// <summary>
        /// Gets or sets the total contingency hours for the project. Contingency hours are calculated 
        /// by multiplying the <see cref="TotalHours"/> by the <see cref="ContingencyRate"/>.
        /// </summary>
        /// <value>
        /// The total contingency hours for the project.
        /// </value>
        [DefaultValue(0)]
        [BsonElement("ch")]
        public int ContingencyHours { get; set; }

        /// <summary>
        /// Gets or sets the total contingency weeks for the project. Contingency weeks are calculated 
        /// by multiplying the <see cref="TotalWeeks"/> by the <see cref="ContingencyRate"/>.
        /// </summary>
        /// <value>
        /// The total contingency weeks for the project.
        /// </value>
        [DefaultValue(0)]
        [BsonElement("cw")]
        public double ContingencyWeeks { get; set; }

        /// <summary>
        /// Gets or set the organization this project belongs to.
        /// </summary>
        [Required]
        [BsonElement("o_")]
        public string OrganizationId { get; set; }

        /// <summary>
        /// Get or Set security key used for public access
        /// </summary>
        [BsonElement("sk")]
        public string SecurityKey { get; set; }

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


        /// <summary>
        /// Gets or sets the assumptions used when estimating the project.
        /// </summary>
        /// <value>
        /// The assumptions used when estimating the project.
        /// </value>
        [BsonElement("_a")]
        public List<string> Assumptions { get; set; }

        /// <summary>
        /// Gets or sets the factors for the project.
        /// </summary>
        /// <value>
        /// The factors for the project.
        /// </value>
        [BsonElement("_f")]
        public List<Factor> Factors { get; set; }

        /// <summary>
        /// Gets or sets the estimation sections for the project.
        /// </summary>
        /// <value>
        /// The estimation sections for the project.
        /// </value>
        [BsonElement("_s")]
        public List<Section> Sections { get; set; }
    }
}
