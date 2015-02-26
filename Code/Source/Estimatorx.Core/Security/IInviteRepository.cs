using System.Linq;
using Estimatorx.Core.Providers;

namespace Estimatorx.Core.Security
{
    public interface IInviteRepository
        : IEntityRepository<Invite, string>
    {
        IQueryable<Invite> FindByOrganization(string organizationId);
    }
}