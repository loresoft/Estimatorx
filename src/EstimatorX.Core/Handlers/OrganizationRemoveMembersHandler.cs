
using EstimatorX.Core.Commands;
using EstimatorX.Core.Repositories;

using MediatR.CommandQuery.Handlers;
using MediatR.CommandQuery.Models;

using Microsoft.Extensions.Logging;

namespace EstimatorX.Core.Handlers;

public class OrganizationRemoveMembersHandler : RequestHandlerBase<OrganizationRemoveMembersCommand, CompleteModel>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUserRepository _userRepository;

    public OrganizationRemoveMembersHandler(ILoggerFactory loggerFactory, IOrganizationRepository organizationRepository, IUserRepository userRepository) : base(loggerFactory)
    {
        _organizationRepository = organizationRepository;
        _userRepository = userRepository;
    }

    protected override async Task<CompleteModel> Process(OrganizationRemoveMembersCommand request, CancellationToken cancellationToken)
    {
        string organizationId = request.Id;

        Logger.LogDebug("Removing organization '{OrganizationId}' membership", organizationId);

        // users that are part of deleted org
        var existingUsers = await _userRepository.FindAllAsync(u => u.Organizations.Any(o => o.Id == organizationId), cancellationToken);
        if (existingUsers.Count == 0)
            return new CompleteModel { Successful = true };

        // remove org from removed members
        foreach (var user in existingUsers)
        {
            user.Organizations.RemoveAll(o => o.Id == organizationId);
            user.Updated = request.Activated;
            user.UpdatedBy = request.ActivatedBy;

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        Logger.LogDebug("Removed {RemoveCount} users membership", existingUsers.Count);

        return new CompleteModel { Successful = true };
    }
}
