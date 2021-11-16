
using EstimatorX.Core.Commands;
using EstimatorX.Core.Comparison;
using EstimatorX.Core.Entities;
using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Models;

using MediatR.CommandQuery.Handlers;
using MediatR.CommandQuery.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Handlers;

public class OrganizationUpdateMembersHandler : RequestHandlerBase<OrganizationUpdateMembersCommand, CompleteModel>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUserRepository _userRepository;

    public OrganizationUpdateMembersHandler(ILoggerFactory loggerFactory, IOrganizationRepository organizationRepository, IUserRepository userRepository) : base(loggerFactory)
    {
        _organizationRepository = organizationRepository;
        _userRepository = userRepository;
    }

    protected override async Task<CompleteModel> Process(OrganizationUpdateMembersCommand request, CancellationToken cancellationToken)
    {
        string organizationId = request.Id;

        Logger.LogDebug("Updating organization '{OrganizationId}' membership", organizationId);

        var organization = await _organizationRepository.FindAsync(organizationId, cancellationToken: cancellationToken);
        if (organization == null)
            return new CompleteModel { Successful = true };

        var usersIds = organization.Members.Select(m => m.Id).ToList();
        if (usersIds.Count == 0)
            return new CompleteModel { Successful = true };

        // users that should be part of org
        var currentUsers = await _userRepository.FindAllAsync(u => usersIds.Contains(u.Id), cancellationToken);

        // users that are/were part of org
        var existingUsers = await _userRepository.FindAllAsync(u => u.Organizations.Any(o => o.Id == organizationId), cancellationToken);

        // compute delta
        var comparer = Equality<User>.CreateComparer(p => p.Id);
        var delta = DeltaCompare.Compare(existingUsers, currentUsers, comparer);

        // add org to new members
        foreach (var user in delta.Created)
        {
            user.Organizations.Add(new IdentifierName { Id = organization.Id, Name = organization.Name });
            user.Updated = request.Activated;
            user.UpdatedBy = request.ActivatedBy;

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        // remove org from removed members
        foreach (var user in delta.Deleted)
        {
            user.Organizations.RemoveAll(o => o.Id == organizationId);
            user.Updated = request.Activated;
            user.UpdatedBy = request.ActivatedBy;

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        // check existing for org name change
        foreach (var user in delta.Matched)
        {
            var renamedOrganizations = user.Organizations
                .Where(o => o.Id == organizationId && o.Name != organization.Name)
                .ToList();

            if (renamedOrganizations.Count == 0)
                continue;

            renamedOrganizations.ForEach(o => o.Name = organization.Name);

            user.Updated = request.Activated;
            user.UpdatedBy = request.ActivatedBy;

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        Logger.LogDebug("Updated {UpdateCount} users membership; Removed {RemoveCount} users membership", delta.Created.Count, delta.Deleted.Count);

        return new CompleteModel { Successful = true };
    }
}
