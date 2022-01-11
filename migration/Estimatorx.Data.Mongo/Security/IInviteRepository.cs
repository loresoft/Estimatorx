using System.Linq;

using Estimatorx.Data.Mongo.Providers;

namespace Estimatorx.Data.Mongo.Security
{
    public interface IInviteRepository
        : IEntityRepository<Invite, string>
    {
        IQueryable<Invite> FindByOrganization(string organizationId);
    }
}