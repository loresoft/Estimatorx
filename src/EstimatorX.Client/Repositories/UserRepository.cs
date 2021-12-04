
using EstimatorX.Client.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories;

public class UserRepository : RepositoryBase<UserModel, UserSummary>
{
    public UserRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/user";
    }
}
