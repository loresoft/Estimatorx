using System.Security.Principal;

using AutoMapper;

using Cosmos.Abstracts;

using EstimatorX.Core.Entities;
using EstimatorX.Core.Query;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public abstract class OrganizationServiceBase<TRepository, TEntity, TModel> : ServiceBase<TRepository, TModel>
    where TRepository : ICosmosRepository<TEntity>
    where TEntity : EntityBase, IHaveOrganization, new()
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
            throw new DomainException(System.Net.HttpStatusCode.Unauthorized, "Not authorized to delete this entity");

        await Repository.DeleteAsync(id, cancellationToken: cancellationToken);
    }

    public override async Task<TModel> Load(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        var entity = await Repository.FindAsync(id, partitionKey, cancellationToken: cancellationToken);
        if (entity == null)
            return null; // throw NotFound?

        if (!await HasAccess(principal, entity, cancellationToken))
            throw new DomainException(System.Net.HttpStatusCode.Unauthorized, "Not authorized to load this entity");

        return Mapper.Map<TModel>(entity);
    }

    public override async Task<TModel> Save(TModel model, IPrincipal principal, CancellationToken cancellationToken)
    {
        var entity = await Repository.FindAsync(model.Id, cancellationToken: cancellationToken);
        if (entity == null)
            entity = new TEntity();

        if (!await HasAccess(principal, entity, cancellationToken))
            throw new DomainException(System.Net.HttpStatusCode.Unauthorized, "Not authorized to save this entity");

        Mapper.Map(model, entity);

        UpdateTracking(entity, principal);

        var result = await Repository.SaveAsync(entity, cancellationToken);
        if (result == null)
            return null; // throw error?

        return Mapper.Map<TModel>(result);

    }

    public override async Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryRequest, IPrincipal principal, CancellationToken cancellationToken)
    {
        var securyQuery = await SecureQuery(principal, cancellationToken);

        if (queryRequest.Search.HasValue())
            securyQuery = SearchQuery(securyQuery, queryRequest.Search);

        if (queryRequest.Organization.HasValue())
            securyQuery = securyQuery.Where(u => u.OrganizationId == queryRequest.Organization);

        return await securyQuery
            .ToDataResult<TEntity, TResult>(
                config => config
                    .Request(queryRequest)
                    .Mapper(Mapper.ConfigurationProvider),
                cancellationToken
            );
    }

    protected async Task<bool> HasAccess(IPrincipal principal, TEntity entity, CancellationToken cancellationToken)
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

    protected async Task<IQueryable<TEntity>> SecureQuery(IPrincipal principal, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser(principal, cancellationToken);
        if (currentUser == null)
            return null;

        // access to organizations or user id
        var access = currentUser.Organizations
            .Select(o => o.Id)
            .ToList();

        access.Add(currentUser.PrivateKey);

        var query = await Repository.GetQueryableAsync();

        return query
            .Where(t => access.Contains(t.OrganizationId));
    }

    protected abstract IQueryable<TEntity> SearchQuery(IQueryable<TEntity> query, string searchTerm);
}
