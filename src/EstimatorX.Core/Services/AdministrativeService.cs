using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Cosmos.Abstracts;

using EstimatorX.Core.Query;
using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public class AdministrativeService : IAdministrativeService, IServiceTransient
{
    private readonly ILogger<AdministrativeService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public AdministrativeService(ILogger<AdministrativeService> logger, IMapper mapper, IUserRepository userRepository, IOrganizationRepository organizationRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
    }


    public async Task<Organization> LoadOrganization(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        return await Load(_organizationRepository, id, partitionKey, principal, cancellationToken);
    }

    public async Task<QueryResult<OrganizationSummary>> SearchOrganizations(QueryRequest queryRequest, IPrincipal principal, CancellationToken cancellationToken)
    {
        Expression<Func<Organization, bool>> predicate = null;

        if (queryRequest.Search.HasValue())
            predicate = o => o.Name.Contains(queryRequest.Search, StringComparison.InvariantCultureIgnoreCase)
                || o.Description.Contains(queryRequest.Search, StringComparison.InvariantCultureIgnoreCase);


        return await Search<Organization, OrganizationSummary>(
            _organizationRepository,
            predicate,
            queryRequest,
            principal,
            cancellationToken
        );
    }


    public async Task<User> LoadUser(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        return await Load(_userRepository, id, partitionKey, principal, cancellationToken);
    }

    public async Task<QueryResult<UserSummary>> SearchUsers(QueryRequest queryRequest, IPrincipal principal, CancellationToken cancellationToken)
    {
        Expression<Func<User, bool>> predicate = null;

        if (queryRequest.Search.HasValue())
            predicate = u => u.Name.Contains(queryRequest.Search, StringComparison.InvariantCultureIgnoreCase)
                || u.Email.Contains(queryRequest.Search, StringComparison.InvariantCultureIgnoreCase);


        return await Search<User, UserSummary>(
            _userRepository,
            predicate,
            queryRequest,
            principal,
            cancellationToken
        );
    }


    private async Task<QueryResult<TResponse>> Search<TEntity, TResponse>(ICosmosRepository<TEntity> repository, Expression<Func<TEntity, bool>> predicate, QueryRequest queryRequest, IPrincipal principal, CancellationToken cancellationToken)
        where TEntity : class, IHaveIdentifier
    {
        var currentUser = await CurrentUser(principal, cancellationToken);
        if (currentUser == null)
            throw new DomainException(System.Net.HttpStatusCode.Unauthorized, "Could not load current user");


        bool isAdministrator = currentUser.Roles.Any(r => string.Equals(r, Shared.Security.Roles.Administrators, StringComparison.OrdinalIgnoreCase));
        if (!isAdministrator)
            throw new DomainException(System.Net.HttpStatusCode.Unauthorized, "Access denied");

        // find organization for current user only
        var query = await repository.GetQueryableAsync();
        var securyQuery = query.AsQueryable();

        if (predicate != null)
            securyQuery = securyQuery.Where(predicate);

        return await securyQuery
            .ToDataResult(
                (QueryOptionsBuilder<TEntity, TResponse> config) => config
                    .Request(queryRequest)
                    .Mapper(_mapper.ConfigurationProvider),
                cancellationToken
            );
    }

    private async Task<TEntity> Load<TEntity>(ICosmosRepository<TEntity> repository, string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser(principal, cancellationToken);
        if (currentUser == null)
            throw new DomainException(System.Net.HttpStatusCode.Unauthorized, "Could not load current user");

        bool isAdministrator = currentUser.Roles.Any(r => string.Equals(r, Shared.Security.Roles.Administrators, StringComparison.OrdinalIgnoreCase));
        if (!isAdministrator)
            throw new DomainException(System.Net.HttpStatusCode.Unauthorized, "Access denied");

        var entity = await repository.FindAsync(id, partitionKey, cancellationToken);
        if (entity == null)
            return default;

        return entity;
    }


    private async Task<User> CurrentUser(IPrincipal principal, CancellationToken cancellationToken = default)
    {
        string userId = principal.GetUserId();

        return userId.HasValue()
            ? await _userRepository.FindAsync(userId, cancellationToken: cancellationToken)
            : default;
    }

}
