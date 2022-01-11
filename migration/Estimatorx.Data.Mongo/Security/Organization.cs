using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Estimatorx.Data.Mongo.Security
{
    public class Organization
    {
        public Organization()
        {
            Owners = new HashSet<string>();
        }

        /// <summary>
        /// Gets or sets the identifier for the organization.
        /// </summary>
        /// <value>
        /// The identifier for the model.
        /// </value>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the organization.
        /// </summary>
        /// <value>
        /// The name of the organization.
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the organization.
        /// </summary>
        /// <value>
        /// The description of the organization.
        /// </value>
        public string Description { get; set; }

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
        /// Gets or sets the user ids that can edit the organization.
        /// </summary>
        /// <value>
        /// The user ids that can edit the organization.
        /// </value>
        public HashSet<string> Owners { get; set; }
    }
}
