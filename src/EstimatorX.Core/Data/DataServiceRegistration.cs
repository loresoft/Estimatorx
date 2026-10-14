using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Data;

public static class DataServiceRegistration
{
    public const string ConnectionStringName = "EstimatorX";

    public const string SqlServerMigrationsAssembly = "EstimatorX.Migrations.SqlServer";
    public const string SqliteMigrationsAssembly = "EstimatorX.Migrations.Sqlite";
    public const string PostgreSqlMigrationsAssembly = "EstimatorX.Migrations.PostgreSql";

    [RegisterServices]
    public static void Register(IServiceCollection services)
    {
        services
            .AddOptions<DatabaseOptions>()
            .BindConfiguration(DatabaseOptions.SectionName);

        services.AddDbContextPool<EstimatorContext>(
            optionsAction: static (serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                var provider = ResolveProvider(configuration);
                var connectionString = ResolveConnectionString(configuration, provider);

                options.UseEstimatorProvider(provider, connectionString);
            }
        );

        services.AddHostedService<DatabaseMigration>();
    }

    public static DatabaseProvider ResolveProvider(
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var provider = configuration[$"{DatabaseOptions.SectionName}:{nameof(DatabaseOptions.Provider)}"];
        return ParseProvider(provider);
    }

    public static string ResolveConnectionString(
        IConfiguration configuration,
        DatabaseProvider provider)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString = configuration[$"{DatabaseOptions.SectionName}:{nameof(DatabaseOptions.ConnectionString)}"];
        if (!string.IsNullOrWhiteSpace(connectionString))
            return connectionString;

        connectionString = configuration.GetConnectionString(ConnectionStringName);
        if (!string.IsNullOrWhiteSpace(connectionString))
            return connectionString;

        if (provider == DatabaseProvider.SQLite)
            return DatabaseOptions.DefaultConnectionString;

        throw new InvalidOperationException(
            $"A connection string is required for database provider '{provider}'. " +
            "Set the 'Database__ConnectionString' environment variable or the 'Database:ConnectionString' configuration value.");
    }

    public static TBuilder UseEstimatorProvider<TBuilder>(
        this TBuilder options,
        DatabaseProvider provider,
        string connectionString)
        where TBuilder : DbContextOptionsBuilder
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        if (provider == DatabaseProvider.SqlServer)
            UseSqlServer(options, connectionString);
        else if (provider == DatabaseProvider.SQLite)
            UseSqlite(options, connectionString);
        else if (provider == DatabaseProvider.PostgreSQL)
            UsePostgres(options, connectionString);
        else
            throw new ArgumentOutOfRangeException(nameof(provider), provider, $"Unsupported database provider '{provider}'.");

        return options;
    }

    public static DatabaseProvider ParseProvider(string? provider)
    {
        if (string.IsNullOrWhiteSpace(provider))
            return DatabaseProvider.SQLite;

        return provider.Trim().ToLowerInvariant() switch
        {
            "sqlserver" or "mssql" => DatabaseProvider.SqlServer,
            "sqlite" => DatabaseProvider.SQLite,
            "postgresql" or "postgres" or "npgsql" => DatabaseProvider.PostgreSQL,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, "Unsupported database provider.")
        };
    }


    private static void UsePostgres<TBuilder>(TBuilder options, string connectionString)
        where TBuilder : DbContextOptionsBuilder
    {
        options.UseNpgsql(
            connectionString,
            options => options
                .EnableRetryOnFailure()
                .CommandTimeout(300) // 5 minutes
                .UseParameterizedCollectionMode(ParameterTranslationMode.MultipleParameters)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .MigrationsAssembly(PostgreSqlMigrationsAssembly)
        );
    }

    private static void UseSqlite<TBuilder>(TBuilder options, string connectionString)
        where TBuilder : DbContextOptionsBuilder
    {
        options.UseSqlite(
            connectionString,
            options => options
                .CommandTimeout(300) // 5 minutes
                .UseParameterizedCollectionMode(ParameterTranslationMode.MultipleParameters)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .MigrationsAssembly(SqliteMigrationsAssembly)
        );
    }

    private static void UseSqlServer<TBuilder>(TBuilder options, string connectionString)
        where TBuilder : DbContextOptionsBuilder
    {
        options.UseSqlServer(
            connectionString,
            options => options
                .EnableRetryOnFailure()
                .CommandTimeout(300) // 5 minutes
                .UseParameterizedCollectionMode(ParameterTranslationMode.MultipleParameters)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .MigrationsAssembly(SqlServerMigrationsAssembly)

        );
    }

}
