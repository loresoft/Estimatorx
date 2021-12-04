using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface ITemplateService
{
    Task Delete(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);
    Task<TemplateModel> Load(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);
    Task<TemplateModel> Save(TemplateModel model, IPrincipal principal, CancellationToken cancellationToken);
    Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryModel, IPrincipal principal, CancellationToken cancellationToken);
}
