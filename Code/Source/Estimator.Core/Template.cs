using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimator.Core
{
    /// <summary>
    /// A template of factors
    /// </summary>
    public class Template : ModelBase
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

        /// <summary>
        /// Gets or sets the factors for the template.
        /// </summary>
        /// <value>
        /// The factors for the template.
        /// </value>
        public List<Factor> Factors { get; set; }

    }
}
