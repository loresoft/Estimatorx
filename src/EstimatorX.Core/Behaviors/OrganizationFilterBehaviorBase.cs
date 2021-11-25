
using System.Security.Principal;

using EstimatorX.Core.Repositories;
using EstimatorX.Core.Services;
using EstimatorX.Shared.Extensions;

using MediatR;
using MediatR.CommandQuery;
using MediatR.CommandQuery.Behaviors;
using MediatR.CommandQuery.Definitions;
using MediatR.CommandQuery.Queries;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Behaviors;


public abstract class OrganizationFilterBehaviorBase<TKey, TEntityModel, TRequest, TResponse>
    : PipelineBehaviorBase<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUserCache _userCache;

    protected OrganizationFilterBehaviorBase(ILoggerFactory loggerFactory, IUserCache userCache) : base(loggerFactory)
    {
        _userCache = userCache;
    }

    protected virtual string FilterColumn { get; } = "OrganizationId";

    protected virtual async Task<EntityFilter> RewriteFilter(EntityFilter originalFilter, IPrincipal principal)
    {
        var userId = principal.GetUserId();
        if (userId.IsNullOrEmpty())
            throw new DomainException(500, "Could not find current user for the query request.");

        var user = await _userCache.GetCachedUser(userId);
        if (user == null)
            throw new DomainException(500, "Could not find current user for the query request.");

        var organizationIds = user.Organizations
            .Select(o => o.Id)
            .ToList();

        // create OR group for all orgs
        var groupFilter = new EntityFilter
        {
            Logic = EntityFilterLogic.Or,
            Filters = new List<EntityFilter>
            {
                // User Id is used as private group
                new EntityFilter
                {
                    Name = FilterColumn,
                    Value = userId,
                    Operator = EntityFilterOperators.Equal
                }
            }
        };

        // add orgs to group
        foreach (var organizationId in organizationIds)
        {
            var organizationFilter = new EntityFilter
            {
                Name = FilterColumn,
                Value = organizationId,
                Operator = EntityFilterOperators.Equal
            };
            groupFilter.Filters.Add(organizationFilter);
        }

        if (originalFilter == null)
            return groupFilter;

        // if existing filter, create AND group
        var boolFilter = new EntityFilter
        {
            Logic = EntityFilterLogic.And,
            Filters = new List<EntityFilter>
            {
                groupFilter,
                originalFilter
            }
        };

        return boolFilter;
    }
}


public class OrganizationPagedQueryBehavior<TKey, TEntityModel>
    : OrganizationFilterBehaviorBase<TKey, TEntityModel, EntityPagedQuery<TEntityModel>, EntityPagedResult<TEntityModel>>
{
    public OrganizationPagedQueryBehavior(ILoggerFactory loggerFactory, IUserCache userCache) : base(loggerFactory, userCache)
    {
    }

    protected override async Task<EntityPagedResult<TEntityModel>> Process(
        EntityPagedQuery<TEntityModel> request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<EntityPagedResult<TEntityModel>> next)
    {
        // add organization filter
        request.Query.Filter = await RewriteFilter(request.Query.Filter, request.Principal).ConfigureAwait(false);

        // continue pipeline
        return await next().ConfigureAwait(false);
    }
}

public class OrganizationSelectQueryBehavior<TKey, TEntityModel>
    : OrganizationFilterBehaviorBase<TKey, TEntityModel, EntitySelectQuery<TEntityModel>, IReadOnlyCollection<TEntityModel>>
{
    public OrganizationSelectQueryBehavior(ILoggerFactory loggerFactory, IUserCache userCache) : base(loggerFactory, userCache)
    {
    }

    protected override async Task<IReadOnlyCollection<TEntityModel>> Process(
        EntitySelectQuery<TEntityModel> request,
        CancellationToken cancellationToken,
        RequestHandlerDelegate<IReadOnlyCollection<TEntityModel>> next)
    {
        // add organization filter
        request.Select.Filter = await RewriteFilter(request.Select.Filter, request.Principal).ConfigureAwait(false);

        // continue pipeline
        return await next().ConfigureAwait(false);
    }
}
