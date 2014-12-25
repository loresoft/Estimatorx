using System;
using System.Collections.Generic;

namespace Estimatorx.Core
{
    /// <summary>
    /// A project estimate model
    /// </summary>
    public class Project : ProjectSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            HoursPerWeek = 30;
            ContingencyRate = 10;

            Assumptions = new List<Assumption>();
            Factors = new List<Factor>();
            Sections = new List<Section>();
        }

        /// <summary>
        /// Gets or sets the assumptions used when estimating the project.
        /// </summary>
        /// <value>
        /// The assumptions used when estimating the project.
        /// </value>
        public List<Assumption> Assumptions { get; set; }

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
