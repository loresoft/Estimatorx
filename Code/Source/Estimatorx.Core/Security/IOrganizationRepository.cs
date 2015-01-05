using System.Collections.Generic;
using System.Linq;
using Estimatorx.Core.Providers;

namespace Estimatorx.Core.Security
{
    public interface IOrganizationRepository
        : IEntityRepository<Organization, string>
    {
        IQueryable<Organization> FindAll(IEnumerable<string> keys);
    }
}