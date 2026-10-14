using Arbiter.CommandQuery.Handlers;
using Arbiter.CommandQuery.Models;

using EstimatorX.Data;
using EstimatorX.Domain.Commands;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Domain.Handlers;

[RegisterTransient]
public class UserOrganizationDeleteHandler : RequestHandlerBase<UserOrganizationDeleteCommand, CompleteModel>
{
    private readonly EstimatorContext _context;

    public UserOrganizationDeleteHandler(ILoggerFactory loggerFactory, EstimatorContext context) : base(loggerFactory)
    {
        _context = context;
    }

    protected override async ValueTask<CompleteModel?> Process(UserOrganizationDeleteCommand request, CancellationToken cancellationToken = default)
    {
        Logger.LogInformation(
            "Deleting UserOrganization with UserId {UserId} and OrganizationId {OrganizationId}",
            request.UserId, request.OrganizationId);

        var count = await _context.UserOrganizations
            .Where(x => x.UserId == request.UserId && x.OrganizationId == request.OrganizationId)
            .ExecuteDeleteAsync(cancellationToken);

        return CompleteModel.Success($"Deleted {count} UserOrganization(s)");
    }
}
