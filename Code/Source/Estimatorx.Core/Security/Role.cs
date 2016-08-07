using System;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Core.Security
{
    public class Role
        : IRole<string>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

    }
}