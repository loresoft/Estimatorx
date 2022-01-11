using System.Security.Principal;
using System.Threading;

using AutoMapper;

using Cosmos.Abstracts;

using EstimatorX.Core.Query;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public abstract class OrganizationServiceBase<TRepository, TModel> : ServiceBase<TRepository, TModel>
    where TRepository : ICosmosRepository<TModel>
    where TModel : ModelBase, IHaveOrganization, new()
{
    protected OrganizationServiceBase(ILoggerFactory loggerFactory, IMapper mapper, TRepository repository, IUserCache userCache)
        : base(loggerFactory, mapper, repository, userCache)
    {

    }

    public override async Task Delete(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        var entity = await Repository.FindAsync(id, partitionKey, cancellationToken: cancellationToken);
        if (entity == null)
            return;

        if (!await HasAccess(principal, entity, cancellationToken))
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized to delete this entity");

        await Repository.DeleteAsync(id, partitionKey, cancellationToken: cancellationToken);
    }

    public override async Task<TModel> Load(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        var entity = await Repository.FindAsync(id, partitionKey, cancellationToken: cancellationToken);
        if (entity == null)
            return null; // throw NotFound?

        if (!await HasAccess(principal, entity, cancellationToken))
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized to load this entity");

        return entity;
    }

    public override async Task<TModel> Save(string id, string partitionKey, TModel model, IPrincipal principal, CancellationToken cancellationToken)
    {
        var entity = await Repository.FindAsync(id, partitionKey, cancellationToken: cancellationToken);
        if (entity == null)
            entity = new TModel();

        if (!await HasAccess(principal, entity, cancellationToken))
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized to save this entity");

        Mapper.Map(model, entity);

        UpdateTracking(entity, principal);
        await UpdateOrganizationName(entity, principal, cancellationToken);

        var result = await Repository.SaveAsync(entity, cancellationToken);
        if (result == null)
            return null; // throw error?

        // if partition key changes, need to delete old record
        if (partitionKey.HasValue() && partitionKey != result.OrganizationId)
            await Repository.DeleteAsync(id, partitionKey, cancellationToken: cancellationToken);

        return result;

    }


    public override async Task<TModel> Create(TModel model, IPrincipal principal, CancellationToken cancellationToken)
    {
        var entity = new TModel();

        Mapper.Map(model, entity);

        UpdateTracking(entity, principal);

        var result = await Repository.CreateAsync(entity, cancellationToken);
        if (result == null)
            return null; // throw error?

        return result;
    }

    public override async Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryRequest, IPrincipal principal, CancellationToken cancellationToken)
    {
        var securyQuery = await SecureQuery(principal, cancellationToken);

        if (queryRequest.Search.HasValue())
            securyQuery = SearchQuery(securyQuery, queryRequest.Search);

        if (queryRequest.Organization.HasValue())
            securyQuery = securyQuery.Where(u => u.OrganizationId == queryRequest.Organization);

        return await securyQuery
            .ToDataResult<TModel, TResult>(
                config => config
                    .Request(queryRequest)
                    .Mapper(Mapper.ConfigurationProvider),
                cancellationToken
            );
    }

    protected async Task<bool> HasAccess(IPrincipal principal, TModel entity, CancellationToken cancellationToken)
    {
        var user = await CurrentUser(principal, cancellationToken);
        if (user == null)
            return false;

        if (entity == null || entity.OrganizationId.IsNullOrEmpty())
            return true; // allow create

        // user must be member 
        return entity.OrganizationId == user.PrivateKey
            || user.Organizations.Any(o => o.Id == entity.OrganizationId);
    }

    protected async Task<IQueryable<TModel>> SecureQuery(IPrincipal principal, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser(principal, cancellationToken);
        if (currentUser == null)
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized for this organization");

        // access to organizations or user id
        var access = currentUser.Organizations
            .Select(o => o.Id)
            .ToList();

        access.Add(currentUser.PrivateKey);

        var query = await Repository.GetQueryableAsync();

        return query
            .Where(t => access.Contains(t.OrganizationId));
    }

    protected abstract IQueryable<TModel> SearchQuery(IQueryable<TModel> query, string searchTerm);


    protected async Task UpdateOrganizationName(TModel entity, IPrincipal principal, CancellationToken cancellationToken)
    {
        var user = await CurrentUser(principal, cancellationToken);
        if (user == null)
            return;

        if (entity.OrganizationId == user.PrivateKey)
        {
            entity.OrganizationName = "Private";
            return;
        }

        var organization = user.Organizations
            .FirstOrDefault(o => o.Id == entity.OrganizationId);

        entity.OrganizationName = organization?.Name;
    }
}
