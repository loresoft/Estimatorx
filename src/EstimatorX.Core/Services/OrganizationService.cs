using System.Security.Principal;

using AutoMapper;

using EstimatorX.Core.Comparison;
using EstimatorX.Core.Query;
using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Services;

public class OrganizationService : ServiceBase<IOrganizationRepository, Organization>, IOrganizationService, ITransientService
{
    private readonly IUserRepository _userRepository;

    public OrganizationService(ILoggerFactory loggerFactory, IMapper mapper, IOrganizationRepository repository, IUserCache userCache, IUserRepository userRepository)
        : base(loggerFactory, mapper, repository, userCache)
    {
        _userRepository = userRepository;
    }

    public override async Task Delete(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        string userId = principal.GetUserId();

        var organization = await LoadVerifyOwner(id, userId, cancellationToken);

        await Repository.DeleteAsync(organization, cancellationToken);
        await RemoveMembers(id, principal, cancellationToken);
    }

    public override async Task<Organization> Load(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser(principal, cancellationToken);
        if (currentUser == null)
            throw new DomainException(System.Net.HttpStatusCode.Unauthorized, "Could not load current user");

        // user must be member
        if (!currentUser.Organizations.Any(o => o.Id == id))
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized to load this organization");

        var organization = await Repository.FindAsync(id, partitionKey, cancellationToken);
        if (organization == null)
            return null;

        return organization;
    }

    public override async Task<Organization> Save(string id, string partitionKey, Organization model, IPrincipal principal, CancellationToken cancellationToken)
    {
        string userId = principal.GetUserId();

        var organization = await Repository.FindAsync(model.Id, cancellationToken: cancellationToken);
        if (organization != null && !organization.Members.Any(m => m.IsOwner && m.Id == userId))
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized to save this organization");

        if (organization == null)
            organization = new Organization();

        Mapper.Map(model, organization);

        UpdateTracking(organization, principal);

        var result = await Repository.SaveAsync(organization, cancellationToken);
        if (result == null)
            throw new DomainException(System.Net.HttpStatusCode.InternalServerError, "Failed to save organization");

        await UpdateMembers(result, principal, cancellationToken);

        return result;
    }

    public override async Task<Organization> Create(Organization model, IPrincipal principal, CancellationToken cancellationToken)
    {
        string userId = principal.GetUserId();
        var organization = new Organization();

        Mapper.Map(model, organization);

        UpdateTracking(organization, principal);

        var result = await Repository.CreateAsync(organization, cancellationToken);
        if (result == null)
            throw new DomainException(System.Net.HttpStatusCode.InternalServerError, "Failed to save organization");

        await UpdateMembers(result, principal, cancellationToken);

        return result;
    }

    public override async Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryRequest, IPrincipal principal, CancellationToken cancellationToken)
    {
        var currentUser = await CurrentUser(principal, cancellationToken);
        if (currentUser == null)
            throw new DomainException(System.Net.HttpStatusCode.Unauthorized, "Could not load current user");

        // only return orgs user has access to
        var organizations = currentUser.Organizations
            .Select(o => o.Id)
            .ToList();

        var query = await Repository.GetQueryableAsync();

        // find organization for current user only
        var securyQuery = query.Where(p => organizations.Contains(p.Id));

        if (queryRequest.Search.HasValue())
        {
            securyQuery = securyQuery
                .Where(u =>
                    u.Name.Contains(queryRequest.Search, StringComparison.InvariantCultureIgnoreCase) ||
                    u.Description.Contains(queryRequest.Search, StringComparison.InvariantCultureIgnoreCase)
                );
        }

        return await securyQuery
            .ToDataResult(
                (QueryOptionsBuilder<Organization, TResult> config) => config
                    .Request(queryRequest)
                    .Mapper(Mapper.ConfigurationProvider),
                cancellationToken
            );
    }


    private async Task UpdateMembers(Organization organization, IPrincipal principal, CancellationToken cancellationToken)
    {
        if (organization == null)
            return;

        var id = organization.Id;

        Logger.LogDebug("Updating organization '{OrganizationId}' membership", id);

        var usersIds = organization.Members.Select(m => m.Id).ToList();
        if (usersIds.Count == 0)
            return;

        var activated = DateTimeOffset.UtcNow;
        var activatedBy = principal.GetEmail();

        // users that should be part of org
        var currentUsers = await _userRepository.FindAllAsync(u => usersIds.Contains(u.Id), cancellationToken);

        // users that are/were part of org
        var existingUsers = await _userRepository.FindAllAsync(u => u.Organizations.Any(o => o.Id == id), cancellationToken);

        // compute delta
        var comparer = Equality<User>.CreateComparer(p => p.Id);
        var delta = DeltaCompare.Compare(existingUsers, currentUsers, comparer);

        // add org to new members
        foreach (var user in delta.Created)
        {
            user.Organizations.Add(new IdentifierName { Id = organization.Id, Name = organization.Name });
            user.Updated = activated;
            user.UpdatedBy = activatedBy;

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        // remove org from removed members
        foreach (var user in delta.Deleted)
        {
            user.Organizations.RemoveAll(o => o.Id == id);
            user.Updated = activated;
            user.UpdatedBy = activatedBy;

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        // check existing for org name change
        foreach (var user in delta.Matched)
        {
            var renamedOrganizations = user.Organizations
                .Where(o => o.Id == id && o.Name != organization.Name)
                .ToList();

            if (renamedOrganizations.Count == 0)
                continue;

            renamedOrganizations.ForEach(o => o.Name = organization.Name);
            user.Updated = activated;
            user.UpdatedBy = activatedBy;

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        Logger.LogDebug("Updated {UpdateCount} users membership; Removed {RemoveCount} users membership", delta.Created.Count, delta.Deleted.Count);
    }

    private async Task RemoveMembers(string id, IPrincipal principal, CancellationToken cancellationToken)
    {
        Logger.LogDebug("Removing organization '{OrganizationId}' membership", id);

        // users that are part of deleted org
        var existingUsers = await _userRepository.FindAllAsync(u => u.Organizations.Any(o => o.Id == id), cancellationToken);
        if (existingUsers.Count == 0)
            return;

        var activated = DateTimeOffset.UtcNow;
        var activatedBy = principal.GetEmail();

        // remove org from removed members
        foreach (var user in existingUsers)
        {
            user.Organizations.RemoveAll(o => o.Id == id);
            user.Updated = activated;
            user.UpdatedBy = activatedBy;

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        Logger.LogDebug("Removed {RemoveCount} users membership", existingUsers.Count);
    }


    private async Task<Organization> LoadVerifyOwner(string id, string userId, CancellationToken cancellationToken)
    {
        var organization = await Repository.FindAsync(id, cancellationToken: cancellationToken);
        if (organization == null)
            return null;

        if (!organization.Members.Any(m => m.IsOwner && m.Id == userId))
            throw new DomainException(System.Net.HttpStatusCode.Forbidden, "Not authorized owner for this organization");

        return organization;
    }

}
