using System.Text.Json;

using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Stores;

[RegisterScoped]
public class OrganizationStore : StoreEditBase<Organization, OrganizationRepository>
{
    public OrganizationStore(ILoggerFactory loggerFactory, OrganizationRepository repository, JsonSerializerOptions serializerOptions)
        : base(loggerFactory, repository, serializerOptions)
    {
    }
}
