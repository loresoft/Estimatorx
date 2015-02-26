using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimatorx.Core.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class Invite
    {
        /// <summary>
        /// Gets or sets the identifier for the invite.
        /// </summary>
        /// <value>
        /// The identifier for the model.
        /// </value>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Email of the invite.
        /// </summary>
        /// <value>
        /// The Email of the invite.
        /// </value>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier of the invite.
        /// </summary>
        [Required]
        public string OrganizationId { get; set; }

        /// <summary>
        /// Get or Set security key used for access
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

    }
}
