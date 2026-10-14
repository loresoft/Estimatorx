#pragma warning disable IDE0130 // Namespace does not match folder structure

using Arbiter.CommandQuery.Extensions;
using Arbiter.CommandQuery.Handlers;
using Arbiter.Mediation;

using EstimatorX.Data;
using EstimatorX.Data.Entities;
using EstimatorX.Domain.Commands;
using EstimatorX.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Domain.Handlers;

[RegisterTransient<IRequestHandler<AuthorizationCommand, AuthorizationModel>>]
public class AuthorizationHandler : RequestHandlerBase<AuthorizationCommand, AuthorizationModel>
{
    private readonly EstimatorContext _dataContext;

    public AuthorizationHandler(ILoggerFactory loggerFactory, EstimatorContext dataContext) : base(loggerFactory)
    {
        _dataContext = dataContext;
    }

    protected override async ValueTask<AuthorizationModel?> Process(AuthorizationCommand request, CancellationToken cancellationToken)
    {
        User user;
        try
        {
            user = await ResolveUser(request, cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            // concurrent sign-in created the same user or provider link, retry once
            Logger.LogWarning(ex, "Concurrency conflict resolving user, retrying");
            _dataContext.ChangeTracker.Clear();

            user = await ResolveUser(request, cancellationToken);
        }

        if (user.IsDisabled)
            throw new UnauthorizedAccessException($"User '{user.Email ?? user.Name ?? user.Id.ToString()}' is disabled");

        return new AuthorizationModel
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email,
            AllowAdministrator = user.IsAdministrator
        };
    }

    private async Task<User> ResolveUser(AuthorizationCommand request, CancellationToken cancellationToken)
    {
        var principal = request.Principal
            ?? throw new ArgumentException("Principal can not be null", nameof(request));

        var userName = principal.Identity?.Name ?? "system";
        var displayName = principal.GetDisplayName() ?? "Anonymous";
        var emailAddress = principal.GetEmail();
        var hasEmail = !emailAddress.IsNullOrWhiteSpace();

        var provider = request.Provider;
        var providerKey = request.ProviderKey;
        var hasProvider = !provider.IsNullOrWhiteSpace() && !providerKey.IsNullOrWhiteSpace();

        if (!hasEmail && !hasProvider)
            throw new ArgumentException("Email claim or external provider identity is required", nameof(request));

        Logger.LogInformation(
            "Resolve User for provider '{Provider}', display name '{DisplayName}', email '{EmailAddress}'",
            provider, displayName, emailAddress);

        User? user = null;

        // 1. match by external provider identity
        if (hasProvider)
        {
            user = await _dataContext.UserProviders
                .Where(p => p.Provider == provider && p.ProviderKey == providerKey)
                .Select(p => p.User)
                .FirstOrDefaultAsync(cancellationToken);
        }

        var linked = user != null;

        // 2. fall back to email match
        if (user == null && hasEmail)
        {
            user = await _dataContext.Users
                .Where(u => u.Email == emailAddress)
                .FirstOrDefaultAsync(cancellationToken);
        }

        // 3. create new user
        if (user == null)
        {
            user = new User
            {
                Name = displayName,
                Email = emailAddress,
                CreatedBy = userName,
                UpdatedBy = userName,
            };
            await _dataContext.Users.AddAsync(user, cancellationToken);

            await AddMatchingOrganizations(user, emailAddress, userName, cancellationToken);

            Logger.LogInformation(
                "Created new user '{UserId}' for provider '{Provider}', display name '{DisplayName}', email '{EmailAddress}'",
                user.Id, provider, displayName, emailAddress);
        }
        else if (user.Name.IsNullOrEmpty() || user.Email.IsNullOrEmpty())
        {
            // only update if current is null. This allows user to override their name or email.
            user.Email ??= emailAddress;
            user.Name ??= displayName;

            user.Updated = DateTimeOffset.UtcNow;
            user.UpdatedBy = userName;
        }

        if (hasProvider && !linked)
        {
            UserProvider userProvider = new()
            {
                Provider = provider!,
                ProviderKey = providerKey!,
                CreatedBy = userName,
                UpdatedBy = userName,
            };
            user.UserProviders.Add(userProvider);

            Logger.LogInformation(
                "Linked user '{UserId}' to provider '{Provider}', display name '{DisplayName}', email '{EmailAddress}'",
                user.Id, provider, displayName, emailAddress);
        }

        UserLogin userLogin = new()
        {
            IpAddress = request.IpAddress,
            UserAgent = request.UserAgent,
            CreatedBy = userName,
            UpdatedBy = userName,
        };
        user.UserLogins.Add(userLogin);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return user;
    }

    private async Task AddMatchingOrganizations(User user, string? emailAddress, string userName, CancellationToken cancellationToken)
    {
        var host = GetEmailHost(emailAddress);
        if (host.IsNullOrWhiteSpace())
            return;

        var organizations = await _dataContext.Organizations
            .AsNoTracking()
            .Where(o => o.HostMatches != null)
            .Select(o => new { o.Id, o.HostMatches })
            .ToListAsync(cancellationToken);

        foreach (var organization in organizations)
        {
            if (!IsHostMatch(host!, organization.HostMatches))
                continue;

            UserOrganization userOrganization = new()
            {
                OrganizationId = organization.Id,
                IsOwner = false,
                CreatedBy = userName,
                UpdatedBy = userName,
            };
            user.UserOrganizations.Add(userOrganization);

            Logger.LogInformation(
                "Added user '{EmailAddress}' to organization '{OrganizationId}' by host match '{Host}'",
                emailAddress, organization.Id, host);
        }
    }

    private static string? GetEmailHost(string? emailAddress)
    {
        if (emailAddress.IsNullOrWhiteSpace())
            return null;

        var index = emailAddress!.LastIndexOf('@');
        if (index < 0 || index == emailAddress.Length - 1)
            return null;

        return emailAddress[(index + 1)..].Trim();
    }

    private static bool IsHostMatch(string host, IList<string>? hostMatches)
    {
        if (hostMatches == null || hostMatches.Count == 0)
            return false;

        foreach (var hostMatch in hostMatches)
        {
            if (hostMatch.IsNullOrWhiteSpace())
                continue;

            var match = hostMatch.Trim().TrimStart('@');
            if (match.Length == 0)
                continue;

            if (string.Equals(host, match, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
