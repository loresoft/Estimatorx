using Arbiter.Services;

using Estimatorx.Data.Cosmos.Repositories;

using EstimatorX.Data;
using EstimatorX.Extensions;
using EstimatorX.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Spectre.Console;
using Spectre.Console.Cli;

using Core = EstimatorX.Data.Entities;
using Domain = EstimatorX.Domain.Models;
using Source = Estimatorx.Data.Cosmos.Models;

namespace Estimatorx.Migration.Commands;

[RegisterTransient]
public class MigrateDataCommand : AsyncCommand
{
    private const int BatchSize = 100;

    private readonly IOrganizationRepository _organizationRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ITemplateRepository _templateRepository;
    private readonly IUserRepository _userRepository;
    private readonly EstimatorContext _dataContext;
    private readonly IProjectCalculator _projectCalculator;
    private readonly ILogger<MigrateDataCommand> _logger;

    private readonly Dictionary<string, long> _organizationKeys = new(StringComparer.Ordinal);
    private readonly Dictionary<string, long> _userKeys = new(StringComparer.Ordinal);
    private readonly Dictionary<string, long> _userEmails = new(StringComparer.OrdinalIgnoreCase);

    public MigrateDataCommand(
        IOrganizationRepository organizationRepository,
        IProjectRepository projectRepository,
        ITemplateRepository templateRepository,
        IUserRepository userRepository,
        EstimatorContext dataContext,
        IProjectCalculator projectCalculator,
        ILogger<MigrateDataCommand> logger)
    {
        _organizationRepository = organizationRepository;
        _projectRepository = projectRepository;
        _templateRepository = templateRepository;
        _userRepository = userRepository;
        _dataContext = dataContext;
        _projectCalculator = projectCalculator;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        try
        {
            var organizations = await MigrateOrganizationsAsync(cancellationToken);
            var users = await MigrateUsersAsync(cancellationToken);
            var userOrganizations = await MigrateUserOrganizationsAsync(cancellationToken);
            var projects = await MigrateProjectsAsync(cancellationToken);
            var templates = await MigrateTemplatesAsync(cancellationToken);

            var table = new Table()
                .AddColumn("Entity")
                .AddColumn("Inserted")
                .AddColumn("Updated")
                .AddColumn("Skipped");

            AddRow(table, "Organization", organizations);
            AddRow(table, "User", users);
            AddRow(table, "UserOrganization", userOrganizations);
            AddRow(table, "Project", projects);
            AddRow(table, "Template", templates);

            AnsiConsole.Write(table);

            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error migrating data: {Message}", ex.Message);
            return 1;
        }
    }

    private async Task<MigrationResult> MigrateOrganizationsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Migrating Organization ...");

        var result = new MigrationResult();
        var sources = await _organizationRepository.FindAllAsync(_ => true, cancellationToken);

        foreach (var source in sources)
        {
            if (source.Key is not long key || key == 0)
            {
                _logger.LogWarning("Skipping Organization {Id} '{Name}': missing Key", source.Id, source.Name);
                result.Skipped++;
                continue;
            }

            var conflict = await _dataContext.Organizations
                .AnyAsync(o => o.Name == source.Name && o.Id != key, cancellationToken);

            if (conflict)
            {
                _logger.LogWarning("Skipping Organization {Id} '{Name}': name already used by another organization", source.Id, source.Name);
                result.Skipped++;
                continue;
            }

            var target = await _dataContext.Organizations.FindAsync([key], cancellationToken);
            if (target == null)
            {
                target = new Core.Organization { Id = key };
                _dataContext.Organizations.Add(target);
                result.Inserted++;
            }
            else
            {
                result.Updated++;
            }

            target.Name = source.Name;
            target.Description = source.Description;
            target.HostMatches = source.HostMatches is { Count: > 0 } ? [.. source.HostMatches] : null;
            CopyAudit(source, target);

            // save each to surface name conflicts within the source set early
            await _dataContext.SaveChangesAsync(cancellationToken);

            _organizationKeys[source.Id] = key;
        }

        _dataContext.ChangeTracker.Clear();

        _logger.LogInformation("Migrated Organization: {Result}", result);
        return result;
    }

    private async Task<MigrationResult> MigrateUsersAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Migrating User ...");

        var result = new MigrationResult();
        var sources = await _userRepository.FindAllAsync(_ => true, cancellationToken);
        var pending = 0;

        var existingUsers = await _dataContext.Users
            .AsNoTracking()
            .Where(u => u.Email != null)
            .Select(u => new { u.Id, u.Email })
            .ToListAsync(cancellationToken);

        foreach (var existingUser in existingUsers)
            _userEmails.TryAdd(existingUser.Email!.Trim(), existingUser.Id);

        foreach (var source in sources)
        {
            if (source.Key is not long key || key == 0)
            {
                _logger.LogWarning("Skipping User {Id} '{Email}': missing Key", source.Id, source.Email);
                result.Skipped++;
                continue;
            }

            var email = string.IsNullOrWhiteSpace(source.Email) ? null : source.Email.Trim();

            if (!string.IsNullOrWhiteSpace(email)
                && _userEmails.TryGetValue(email, out var existingKey)
                && existingKey != key)
            {
                _logger.LogWarning("Skipping User {Id} '{Email}': duplicate email of user {ExistingKey}", source.Id, source.Email, existingKey);
                _userKeys[source.Id] = existingKey;
                result.Skipped++;
                continue;
            }

            var target = await _dataContext.Users.FindAsync([key], cancellationToken);
            if (target == null)
            {
                target = new Core.User { Id = key };
                _dataContext.Users.Add(target);
                result.Inserted++;
            }
            else
            {
                result.Updated++;
            }

            target.Email = email;
            target.Name = source.Name;
            target.IsAdministrator = source.Roles?.Any(r => string.Equals(r, "Administrator", StringComparison.OrdinalIgnoreCase)) == true;
            target.IsDisabled = false;

            CopyAudit(source, target);

            _userKeys[source.Id] = key;
            if (!string.IsNullOrWhiteSpace(email))
                _userEmails[email] = key;

            pending = await SaveBatchAsync(pending + 1, cancellationToken);
        }

        await SaveBatchAsync(BatchSize, cancellationToken);

        _logger.LogInformation("Migrated User: {Result}", result);
        return result;
    }

    private async Task<MigrationResult> MigrateUserOrganizationsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Migrating UserOrganization ...");

        var result = new MigrationResult();

        // (userKey, organizationKey) -> isOwner
        var links = new Dictionary<(long UserId, long OrganizationId), bool>();

        var organizations = await _organizationRepository.FindAllAsync(_ => true, cancellationToken);
        foreach (var organization in organizations)
        {
            if (organization.Id == null || !_organizationKeys.TryGetValue(organization.Id, out var organizationKey))
                continue;

            foreach (var member in organization.Members ?? [])
            {
                if (member.Id == null || !_userKeys.TryGetValue(member.Id, out var userKey))
                {
                    _logger.LogWarning("Skipping UserOrganization for Organization {OrganizationId}: user {UserId} not found", organization.Id, member.Id);
                    result.Skipped++;
                    continue;
                }

                var linkKey = (userKey, organizationKey);
                links[linkKey] = (links.TryGetValue(linkKey, out var owner) && owner) || member.IsOwner;
            }
        }

        var users = await _userRepository.FindAllAsync(_ => true, cancellationToken);
        foreach (var user in users)
        {
            if (user.Id == null || !_userKeys.TryGetValue(user.Id, out var userKey))
                continue;

            foreach (var organization in user.Organizations ?? [])
            {
                if (organization.Id == null || !_organizationKeys.TryGetValue(organization.Id, out var organizationKey))
                {
                    _logger.LogWarning("Skipping UserOrganization for User {UserId}: organization {OrganizationId} not found", user.Id, organization.Id);
                    result.Skipped++;
                    continue;
                }

                links.TryAdd((userKey, organizationKey), false);
            }
        }

        var existing = await _dataContext.UserOrganizations
            .ToDictionaryAsync(u => (u.UserId, u.OrganizationId), cancellationToken);

        var pending = 0;
        foreach (var ((userId, organizationId), isOwner) in links)
        {
            if (existing.TryGetValue((userId, organizationId), out var target))
            {
                result.Updated++;
            }
            else
            {
                target = new Core.UserOrganization
                {
                    UserId = userId,
                    OrganizationId = organizationId,
                    CreatedBy = "migration",
                };
                _dataContext.UserOrganizations.Add(target);
                result.Inserted++;
            }

            target.IsOwner = isOwner;
            target.Updated = DateTimeOffset.UtcNow;
            target.UpdatedBy = "migration";

            pending = await SaveBatchAsync(pending + 1, cancellationToken);
        }

        await SaveBatchAsync(BatchSize, cancellationToken);

        _logger.LogInformation("Migrated UserOrganization: {Result}", result);
        return result;
    }

    private async Task<MigrationResult> MigrateProjectsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Migrating Project ...");

        var result = new MigrationResult();
        var sources = await _projectRepository.FindAllAsync(_ => true, cancellationToken);
        var pending = 0;

        foreach (var source in sources)
        {
            if (!TryResolveEstimate(source, "Project", out var key, out var organizationId, out var userId))
            {
                result.Skipped++;
                continue;
            }

            var target = await _dataContext.Projects.FindAsync([key], cancellationToken);
            if (target == null)
            {
                target = new Core.Project { Id = key };
                _dataContext.Projects.Add(target);
                result.Inserted++;
            }
            else
            {
                result.Updated++;
            }

            target.Name = source.Name;
            target.Description = source.Description.Truncate(2000);
            target.OrganizationId = organizationId;
            target.UserId = userId ?? ResolveUser(source.CreatedBy);
            target.SecurityKey = TokenService.GenerateCode();
            target.Settings = MapSettings(source.Settings);
            target.Epics = MapEpics(source.Epics);
            CopyAudit(source, target);

            _projectCalculator.UpdateProject(target);

            pending = await SaveBatchAsync(pending + 1, cancellationToken);
        }

        await SaveBatchAsync(BatchSize, cancellationToken);

        _logger.LogInformation("Migrated Project: {Result}", result);
        return result;
    }

    private async Task<MigrationResult> MigrateTemplatesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Migrating Template ...");

        var result = new MigrationResult();
        var sources = await _templateRepository.FindAllAsync(_ => true, cancellationToken);
        var pending = 0;

        foreach (var source in sources)
        {
            if (!TryResolveEstimate(source, "Template", out var key, out var organizationId, out var userId))
            {
                result.Skipped++;
                continue;
            }

            var target = await _dataContext.Templates.FindAsync([key], cancellationToken);
            if (target == null)
            {
                target = new Core.Template { Id = key };
                _dataContext.Templates.Add(target);
                result.Inserted++;
            }
            else
            {
                result.Updated++;
            }

            target.Name = source.Name;
            target.Description = source.Description.Truncate(2000);
            target.OrganizationId = organizationId;
            target.UserId = userId ?? ResolveUser(source.CreatedBy);
            target.Settings = MapSettings(source.Settings);
            target.Epics = MapEpics(source.Epics);
            CopyAudit(source, target);

            _projectCalculator.UpdateProject(target);

            pending = await SaveBatchAsync(pending + 1, cancellationToken);
        }

        await SaveBatchAsync(BatchSize, cancellationToken);

        _logger.LogInformation("Migrated Template: {Result}", result);
        return result;
    }

    private bool TryResolveEstimate(Source.Project source, string name, out long key, out long? organizationId, out long? userId)
    {
        key = 0;
        organizationId = null;
        userId = null;

        if (source.Key is not long sourceKey || sourceKey == 0)
        {
            _logger.LogWarning("Skipping {Entity} {Id} '{Name}': missing Key", name, source.Id, source.Name);
            return false;
        }

        key = sourceKey;

        if (string.IsNullOrEmpty(source.OrganizationId))
            return true;

        if (_organizationKeys.TryGetValue(source.OrganizationId, out var organizationKey))
        {
            organizationId = organizationKey;
            return true;
        }

        if (_userKeys.TryGetValue(source.OrganizationId, out var userKey))
        {
            userId = userKey;
            return true;
        }

        _logger.LogWarning("{Entity} {Id} '{Name}': organization or user {OrganizationId} not found, setting OrganizationId to null", name, source.Id, source.Name, source.OrganizationId);
        return true;
    }

    private long? ResolveUser(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return _userEmails.TryGetValue(email, out var key) ? key : null;
    }

    private async Task<int> SaveBatchAsync(int pending, CancellationToken cancellationToken)
    {
        if (pending < BatchSize)
            return pending;

        await _dataContext.SaveChangesAsync(cancellationToken);
        _dataContext.ChangeTracker.Clear();

        return 0;
    }

    private static void CopyAudit(Source.ModelBase source, Arbiter.CommandQuery.Definitions.ITrackCreated target)
    {
        target.Created = source.Created;
        target.CreatedBy = source.CreatedBy;

        if (target is Arbiter.CommandQuery.Definitions.ITrackUpdated updated)
        {
            updated.Updated = source.Created;
            updated.UpdatedBy = source.CreatedBy;
        }
    }

    private static Domain.ProjectSettings MapSettings(Source.ProjectSettings? source)
    {
        var target = new Domain.ProjectSettings();
        if (source == null)
            return target;

        target.EstimateRate = source.EstimateRate;
        target.ShowCost = source.EstimateRate > 0;

        target.EffortLevels = source.EffortLevels?
            .Select(e => new Domain.EffortLevel { Effort = e.Effort, Level = e.Level })
            .ToList() ?? [];

        target.RiskLevels = source.RiskLevels?
            .Select(r => new Domain.RiskLevel { Risk = r.Risk, Multiplier = r.Multiplier })
            .ToList() ?? [];

        target.Overhead = source.Overhead?
            .Select(o => new Domain.ProjectOverhead { Name = o.Name, Description = o.Description, Multiplier = o.Multiplier })
            .ToList() ?? [];

        target.Multipliers = source.Multipliers?
            .Select(m => new Domain.EstimateMultiplier
            {
                Confidence = (Domain.ConfidenceScale)(int)m.Confidence,
                Clarity = (Domain.ClarityScale)(int)m.Clarity,
                Value = m.Value
            })
            .ToList() ?? [];

        return target;
    }

    private static List<Domain.EpicEstimate> MapEpics(List<Source.EpicEstimate>? source)
    {
        if (source == null)
            return [];

        return source
            .Select(e => new Domain.EpicEstimate
            {
                Name = e.Name,
                Description = e.Description,
                Assumptions = e.Assumptions,
                Features = e.Features?
                    .Select(f =>
                    {
                        var feature = MapEstimate<Domain.FeatureEstimate>(f);
                        feature.Stories = f.Stories?
                            .Select(MapEstimate<Domain.StoryEstimate>)
                            .ToList() ?? [];
                        return feature;
                    })
                    .ToList() ?? [],
                EstimatedTotal = e.EstimatedTotal,
                WeightedTotal = e.WeightedTotal,
                EstimatedCost = e.EstimatedCost,
                WeightedCost = e.WeightedCost,
            })
            .ToList();
    }

    private static T MapEstimate<T>(Source.EstimateBase source)
        where T : Domain.EstimateBase, new()
    {
        return new T
        {
            Name = source.Name,
            Description = source.Description,
            Assumptions = source.Assumptions,
            Estimate = source.Estimate,
            Clarity = source.Clarity.HasValue ? (Domain.ClarityScale)(int)source.Clarity.Value : null,
            Confidence = source.Confidence.HasValue ? (Domain.ConfidenceScale)(int)source.Confidence.Value : null,
            Criticality = source.Criticality.HasValue ? (Domain.Criticality)(int)source.Criticality.Value : null,
            Multiplier = source.Multiplier,
            WeightedEstimate = source.WeightedEstimate,
            EstimatedTotal = source.EstimatedTotal,
            WeightedTotal = source.WeightedTotal,
            RiskLevel = source.RiskLevel,
            EffortLevel = source.EffortLevel,
            EstimatedCost = source.EstimatedCost,
            WeightedCost = source.WeightedCost,
        };
    }

    private static void AddRow(Table table, string name, MigrationResult result)
    {
        table.AddRow(name, result.Inserted.ToString(), result.Updated.ToString(), result.Skipped.ToString());
    }

    private sealed class MigrationResult
    {
        public int Inserted { get; set; }

        public int Updated { get; set; }

        public int Skipped { get; set; }

        public override string ToString() => $"Inserted: {Inserted}, Updated: {Updated}, Skipped: {Skipped}";
    }
}
