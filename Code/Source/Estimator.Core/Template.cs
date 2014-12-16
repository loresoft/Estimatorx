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
    public class Template : TemplateSummary
    {
        /// <summary>
        /// Gets or sets the factors for the template.
        /// </summary>
        /// <value>
        /// The factors for the template.
        /// </value>
        public List<Factor> Factors { get; set; }

    }
}
