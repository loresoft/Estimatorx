using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Estimatorx.Core
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
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the project.
        /// </summary>
        /// <value>
        /// The description of the project.
        /// </value>
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets the hours per week to use when calculating an estimates total number of weeks.
        /// </summary>
        /// <value>
        /// The hours per week to use when calculating an estimates total number of weeks.
        /// </value>
        [DefaultValue(0)]
        public int HoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the contingency percentage rate for the project. Contingency rate is multiplied
        /// by the <see cref="TotalHours"/> to give the <see cref="ContingencyHours"/>.
        /// </summary>
        /// <value>
        /// The contingency percentage rate for the project.
        /// </value>
        [DefaultValue(0)]
        public double ContingencyRate { get; set; }

        /// <summary>
        /// Gets or sets the total number tasks estimated for the project.
        /// </summary>
        /// <value>
        /// The total number tasks estimated for the project.
        /// </value>
        [DefaultValue(0)]
        public int TotalTasks { get; set; }

        /// <summary>
        /// Gets or sets the total number hours estimated for the project.
        /// </summary>
        /// <value>
        /// The total hours number estimated for the project.
        /// </value>
        [DefaultValue(0)]
        public int TotalHours { get; set; }

        /// <summary>
        /// Gets or sets the total number weeks estimated for the project.
        /// </summary>
        /// <value>
        /// The total number weeks estimated for the project.
        /// </value>
        [DefaultValue(0)]
        public double TotalWeeks { get; set; }

        /// <summary>
        /// Gets or sets the total contingency hours for the project. Contingency hours are calculated 
        /// by multiplying the <see cref="TotalHours"/> by the <see cref="ContingencyRate"/>.
        /// </summary>
        /// <value>
        /// The total contingency hours for the project.
        /// </value>
        [DefaultValue(0)]
        public int ContingencyHours { get; set; }

        /// <summary>
        /// Gets or sets the total contingency weeks for the project. Contingency weeks are calculated 
        /// by multiplying the <see cref="TotalWeeks"/> by the <see cref="ContingencyRate"/>.
        /// </summary>
        /// <value>
        /// The total contingency weeks for the project.
        /// </value>
        [DefaultValue(0)]
        public double ContingencyWeeks { get; set; }

        /// <summary>
        /// Gets or set the organization this project belongs to.
        /// </summary>
        [Required]
        public string OrganizationId { get; set; }

        /// <summary>
        /// Get or Set security key used for publica access
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// Gets or sets the system create date.
        /// </summary>
        /// <value>
        /// The system create date.
        /// </value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the system create user.
        /// </summary>
        /// <value>
        /// The system create user.
        /// </value>
        public string Creator { get; set; }

        /// <summary>
        /// Gets or sets the system update date.
        /// </summary>
        /// <value>
        /// The system update date.
        /// </value>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Gets or sets the system update user.
        /// </summary>
        /// <value>
        /// The system update user.
        /// </value>
        public string Updater { get; set; }


        /// <summary>
        /// Gets or sets the assumptions used when estimating the project.
        /// </summary>
        /// <value>
        /// The assumptions used when estimating the project.
        /// </value>
        public List<string> Assumptions { get; set; }

        /// <summary>
        /// Gets or sets the factors for the project.
        /// </summary>
        /// <value>
        /// The factors for the project.
        /// </value>
        public List<Factor> Factors { get; set; }

        /// <summary>
        /// Gets or sets the estimation sections for the project.
        /// </summary>
        /// <value>
        /// The estimation sections for the project.
        /// </value>
        public List<Section> Sections { get; set; }
    }
}
