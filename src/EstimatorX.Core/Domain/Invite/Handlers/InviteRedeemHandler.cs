using Arbiter.CommandQuery.EntityFramework.Handlers;
using Arbiter.CommandQuery.Extensions;
using Arbiter.CommandQuery.Models;
using Arbiter.Mapping;
using Arbiter.Services;

using EstimatorX.Data;
using EstimatorX.Data.Entities;
using EstimatorX.Data.Queries;
using EstimatorX.Domain.Commands;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Domain.Handlers;

[RegisterTransient]
public class InviteRedeemHandler : DataContextHandlerBase<EstimatorContext, InviteRedeemCommand, CompleteModel>
{
    public InviteRedeemHandler(
        ILoggerFactory loggerFactory,
        EstimatorContext dataContext,
        IMapper mapper)
        : base(loggerFactory, dataContext, mapper)
    {
    }

    protected override async ValueTask<CompleteModel?> Process(
        InviteRedeemCommand request,
        CancellationToken cancellationToken)
    {
        var token = request.Token;

        // token is stored as a hash in the database for security
        var securityCode = TokenService.HashCode(token);

        // look up the invite by security code
        var invite = await DataContext.Invites
            .FirstOrDefaultAsync(p => p.SecurityCode == securityCode, cancellationToken)
            .ConfigureAwait(false);

        if (invite == null || invite.Expiration < DateTime.UtcNow)
        {
            Logger.LogError("Invite not found or expired for security code {SecurityCode}", securityCode);
            return CompleteModel.Fail("Error: The token is invalid or expired.");
        }

        // prefer the authenticated user, fall back to email match or create a new user
        var user = await ResolveUser(request, invite, cancellationToken).ConfigureAwait(false);

        // if user resolution failed, return an error
        if (user == null)
        {
            Logger.LogError("Failed to create user for invite: {Email}", invite.Email);
            return CompleteModel.Fail("Error: Failed to create user for invite.");
        }

        // remove the invite after redeeming
        DataContext.Invites.Remove(invite);

        // save all changes
        await DataContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        Logger.LogInformation("Redeemed invite for user: {UserId} ({Email})", user.Id, user.Email);

        // opportunistically remove expired invites
        var removed = await DataContext.Invites
            .RemoveExpiredInvites(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (removed > 0)
            Logger.LogInformation("Removed {Count} expired invites", removed);

        return CompleteModel.Success();
    }

    private async Task<User?> ResolveUser(
        InviteRedeemCommand request,
        Invite model,
        CancellationToken cancellationToken)
    {
        User? user = null;

        var userId = request.Principal?.GetUserId<long>();
        if (userId.HasValue)
        {
            user = await DataContext.Users
                .Include(p => p.UserOrganizations)
                .FirstOrDefaultAsync(p => p.Id == userId.Value, cancellationToken)
                .ConfigureAwait(false);

            if (user == null)
            {
                Logger.LogError("Authenticated user {UserId} not found for invite {InviteId}", userId.Value, model.Id);
                return null;
            }
        }

        // fall back to email match if the user is not authenticated or not found
        user ??= await DataContext.Users
            .Include(p => p.UserOrganizations)
            .FirstOrDefaultAsync(p => p.Email == model.Email, cancellationToken)
            .ConfigureAwait(false);

        if (user == null)
        {
            Logger.LogInformation(
                "Creating new user for invite {InviteId}: {Email}",
                model.Id, model.Email);

            user = new User
            {
                Email = model.Email,
                Name = model.Name,
                CreatedBy = "system",
                UpdatedBy = "system",
            };

            await DataContext.Users
                .AddAsync(user, cancellationToken)
                .ConfigureAwait(false);
        }

        if (user.UserOrganizations.Any(p => p.OrganizationId == model.OrganizationId))
            return user;

        Logger.LogInformation(
            "Adding user {Email} to organization {OrganizationId} for invite {InviteId}",
            user.Email, model.OrganizationId, model.Id);

        var userOrganization = new UserOrganization
        {
            User = user,
            OrganizationId = model.OrganizationId,
            CreatedBy = "system",
            UpdatedBy = "system",
        };

        await DataContext.UserOrganizations
            .AddAsync(userOrganization, cancellationToken)
            .ConfigureAwait(false);

        return user;
    }
}
