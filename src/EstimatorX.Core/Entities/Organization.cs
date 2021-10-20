using System.Collections.Generic;
using Cosmos.Abstracts;

namespace EstimatorX.Core.Entities
{
    public class Organization : EntityBase
    {
        public Organization()
        {
            Owners = new HashSet<string>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public HashSet<string> Owners { get; set; }
    }
}
