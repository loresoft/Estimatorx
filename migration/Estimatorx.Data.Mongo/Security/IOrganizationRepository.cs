using System.Collections.Generic;
using System.Linq;

using Estimatorx.Data.Mongo.Providers;

namespace Estimatorx.Data.Mongo.Security
{
    public interface IOrganizationRepository
        : IEntityRepository<Organization, string>
    {
        IQueryable<Organization> FindAll(IEnumerable<string> keys);
    }
}