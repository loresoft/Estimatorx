using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cosmos.Abstracts;
using EstimatorX.Core.Entities;

namespace EstimatorX.Core.Repositories
{
    public interface IUserRepository
        : ICosmosRepository<User>
    {
        Task<IReadOnlyList<User>> OrganizationMembersAsync(string organizationId, CancellationToken cancellationToken = default);
    }
}