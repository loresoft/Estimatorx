using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Estimatorx.Core
{
    /// <summary>
    /// A template of factors
    /// </summary>
    public class Template
    {
        /// <summary>
        /// Gets or sets the identifier for the model.
        /// </summary>
        /// <value>
        /// The identifier for the model.
        /// </value>
        [Required]
        public string Id { get; set; }


        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        /// <value>
        /// The name of the template.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for the template.
        /// </summary>
        /// <value>
        /// The description for the template.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or set the organization this project belongs to.
        /// </summary>
        [Required]
        public string OrganizationId { get; set; }


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
        /// Gets or sets the factors for the template.
        /// </summary>
        /// <value>
        /// The factors for the template.
        /// </value>
        public List<Factor> Factors { get; set; }
    }
}
