using System;

namespace Estimator.Core
{
    public class TemplateSummary : ModelBase
    {
        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        /// <value>
        /// The name of the template.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for the template.
        /// </summary>
        /// <value>
        /// The description for the template.
        /// </value>
        public string Description { get; set; }

    }
}