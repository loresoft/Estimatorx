
using EstimatorX.Client.Services;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories;

public class UserRepository : RepositorySearchBase<User, UserSummary>, IScopedService
{
    public UserRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/user";
    }
}
