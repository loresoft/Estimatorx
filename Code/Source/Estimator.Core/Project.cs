using System;
using System.Collections.Generic;

namespace Estimator.Core
{
    /// <summary>
    /// A project estimate model
    /// </summary>
    public class Project : ModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            Factors = new List<Factor>();
            Sections = new List<Section>();
        }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
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
        public int HoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the contingency percentage rate for the project. Contingency rate is multiplied
        /// by the <see cref="TotalHours"/> to give the <see cref="ContingencyHours"/>.
        /// </summary>
        /// <value>
        /// The contingency percentage rate for the project.
        /// </value>
        public double ContingencyRate { get; set; }

        /// <summary>
        /// Gets or sets the total number tasks estimated for the project.
        /// </summary>
        /// <value>
        /// The total number tasks estimated for the project.
        /// </value>
        public int TotalTasks { get; set; }
        
        /// <summary>
        /// Gets or sets the total number hours estimated for the project.
        /// </summary>
        /// <value>
        /// The total hours number estimated for the project.
        /// </value>
        public int TotalHours { get; set; }
        
        /// <summary>
        /// Gets or sets the total number weeks estimated for the project.
        /// </summary>
        /// <value>
        /// The total number weeks estimated for the project.
        /// </value>
        public double TotalWeeks { get; set; }

        /// <summary>
        /// Gets or sets the total contingency hours for the project. Contingency hours are calculated 
        /// by multiplying the <see cref="TotalHours"/> by the <see cref="ContingencyRate"/>.
        /// </summary>
        /// <value>
        /// The total contingency hours for the project.
        /// </value>
        public int ContingencyHours { get; set; }
        
        /// <summary>
        /// Gets or sets the total contingency weeks for the project. Contingency weeks are calculated 
        /// by multiplying the <see cref="TotalWeeks"/> by the <see cref="ContingencyRate"/>.
        /// </summary>
        /// <value>
        /// The total contingency weeks for the project.
        /// </value>
        public double ContingencyWeeks { get; set; }

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
