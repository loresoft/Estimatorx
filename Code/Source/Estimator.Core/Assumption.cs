using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estimator.Core
{
    /// <summary>
    /// An assumption used when estimating the project.
    /// </summary>
    public class Assumption : ModelBase
    {
        /// <summary>
        /// Gets or sets the text for an assumption used when estimating the project.
        /// </summary>
        /// <value>
        /// The text for the assumption.
        /// </value>
        public string Text { get; set; }
    }
}
