using System;

namespace Estimator.Core
{
    /// <summary>
    /// A base class for the estimator models
    /// </summary>
    public abstract class ModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class.
        /// </summary>
        protected ModelBase()
        {
            Id = Guid.NewGuid();
            IsActive = true;
            SysCreateDate = DateTime.Now;
            SysUpdateDate = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the identifier for the model.
        /// </summary>
        /// <value>
        /// The identifier for the model.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this model is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this model is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the system create date.
        /// </summary>
        /// <value>
        /// The system create date.
        /// </value>
        public DateTime SysCreateDate { get; set; }
        
        /// <summary>
        /// Gets or sets the system create user.
        /// </summary>
        /// <value>
        /// The system create user.
        /// </value>
        public string SysCreateUser { get; set; }
        
        /// <summary>
        /// Gets or sets the system update date.
        /// </summary>
        /// <value>
        /// The system update date.
        /// </value>
        public DateTime SysUpdateDate { get; set; }
        
        /// <summary>
        /// Gets or sets the system update user.
        /// </summary>
        /// <value>
        /// The system update user.
        /// </value>
        public string SysUpdateUser { get; set; }
    }
}