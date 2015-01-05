using System;
using System.Collections.Generic;
using System.Linq;
using Estimatorx.Core.Providers;

namespace Estimatorx.Core.Security
{
    public interface IUserRepository 
        : IEntityRepository<User, string>
    {
        IQueryable<User> FindAll(IEnumerable<string> keys);

        IQueryable<User> OrganizationMembers(string organizationId);
    }
}