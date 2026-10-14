using Arbiter.Services;

using Cosmos.Abstracts;

using Estimatorx.Data.Cosmos.Repositories;

using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Estimatorx.Migration.Commands;

[RegisterTransient]
public class PopulateKeysCommand : AsyncCommand
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ITemplateRepository _templateRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<PopulateKeysCommand> _logger;

    public PopulateKeysCommand(
        IOrganizationRepository organizationRepository,
        IProjectRepository projectRepository,
        ITemplateRepository templateRepository,
        IUserRepository userRepository,
        ILogger<PopulateKeysCommand> logger)
    {
        _organizationRepository = organizationRepository;
        _projectRepository = projectRepository;
        _templateRepository = templateRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        try
        {
            var organizations = await PopulateAsync(
                _organizationRepository,
                e => !e.Key.IsDefined() || e.Key == null || e.Key == 0,
                e => e.Key = Snowflake.Default.NextId(),
                "Organization",
                cancellationToken);

            var users = await PopulateAsync(
                _userRepository,
                e => !e.Key.IsDefined() || e.Key == null || e.Key == 0,
                e => e.Key = Snowflake.Default.NextId(),
                "User",
                cancellationToken);

            var projects = await PopulateAsync(
                _projectRepository,
                e => !e.Key.IsDefined() || e.Key == null || e.Key == 0,
                e => e.Key = Snowflake.Default.NextId(),
                "Project",
                cancellationToken);

            var templates = await PopulateAsync(
                _templateRepository,
                e => !e.Key.IsDefined() || e.Key == null || e.Key == 0,
                e => e.Key = Snowflake.Default.NextId(),
                "Template",
                cancellationToken);

            var table = new Table()
                .AddColumn("Entity")
                .AddColumn("Updated");

            table.AddRow("Organization", organizations.ToString());
            table.AddRow("User", users.ToString());
            table.AddRow("Project", projects.ToString());
            table.AddRow("Template", templates.ToString());

            AnsiConsole.Write(table);

            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error populating keys: {Message}", ex.Message);
            return 1;
        }
    }

    private async Task<int> PopulateAsync<TEntity>(
        ICosmosRepository<TEntity> repository,
        System.Linq.Expressions.Expression<Func<TEntity, bool>> missingKey,
        Action<TEntity> assignKey,
        string name,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        _logger.LogInformation("Populating keys for {Entity} ...", name);

        var entities = await repository.FindAllAsync(missingKey, cancellationToken);

        int count = 0;
        foreach (var entity in entities)
        {
            assignKey(entity);
            await repository.SaveAsync(entity, cancellationToken);
            count++;
        }

        _logger.LogInformation("Populated {Count} keys for {Entity}", count, name);

        return count;
    }
}
