using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Data;

public partial class DatabaseMigration : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<DatabaseOptions> _options;
    private readonly ILogger<DatabaseMigration> _logger;

    public DatabaseMigration(
        IServiceProvider serviceProvider,
        IOptions<DatabaseOptions> options,
        ILogger<DatabaseMigration> logger)
    {
        _serviceProvider = serviceProvider;
        _options = options;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_options.Value.MigrateOnStartup)
        {
            LogMigrationDisabled(_logger);
            return;
        }

        await using var scope = _serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<EstimatorContext>();
        var providerName = context.Database.ProviderName;

        try
        {
            if (context.Database.IsSqlite())
                EnsureDirectory(context.Database.GetConnectionString());

            var migrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);

            var pending = migrations.ToList();
            if (pending.Count == 0)
                LogNoPendingMigrations(_logger, providerName);
            else
                LogApplyingMigrations(_logger, pending.Count, providerName, string.Join(", ", pending));

            // always call MigrateAsync so UseAsyncSeeding runs even when there are no pending migrations
            await context.Database.MigrateAsync(cancellationToken);

            if (pending.Count > 0)
                LogMigrationsApplied(_logger, providerName);
        }
        catch (Exception ex)
        {
            LogMigrationFailed(_logger, ex, providerName);
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static void EnsureDirectory(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            return;

        var builder = new SqliteConnectionStringBuilder(connectionString);
        var dataSource = builder.DataSource;

        if (string.IsNullOrWhiteSpace(dataSource) || dataSource == ":memory:")
            return;

        var directory = Path.GetDirectoryName(Path.GetFullPath(dataSource));
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Database migration on startup is disabled")]
    private static partial void LogMigrationDisabled(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Database is up to date for provider {Provider}")]
    private static partial void LogNoPendingMigrations(ILogger logger, string? provider);

    [LoggerMessage(Level = LogLevel.Information, Message = "Applying {Count} migration(s) for provider {Provider}: {Migrations}")]
    private static partial void LogApplyingMigrations(ILogger logger, int count, string? provider, string migrations);

    [LoggerMessage(Level = LogLevel.Information, Message = "Database migrations applied for provider {Provider}")]
    private static partial void LogMigrationsApplied(ILogger logger, string? provider);

    [LoggerMessage(Level = LogLevel.Error, Message = "Database migration failed for provider {Provider}")]
    private static partial void LogMigrationFailed(ILogger logger, Exception exception, string? provider);
}
