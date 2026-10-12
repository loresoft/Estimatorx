using Arbiter.CommandQuery.Definitions;
using Arbiter.Communication;
using Arbiter.Communication.Email;

using EstimatorX.Core.Tests;
using EstimatorX.Core.Tests.Fixtures;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Testcontainers.PostgreSql;

using XUnit.Hosting.Logging;

namespace EstimatorX.Core.PostgreSql.Tests.Fixtures;

public class DatabaseFixture : TestApplicationFixture, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder("postgis/postgis:16-3.4")
        .WithDatabase("EstimatorX")
        .Build();

    public async ValueTask InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    protected override void ConfigureApplication(HostApplicationBuilder builder)
    {
        base.ConfigureApplication(builder);

        builder.Logging.AddMemoryLogger();

        var services = builder.Services;

        // change database from container default
        string containerConnection = _postgreSqlContainer.GetConnectionString();
        var connectionBuilder = new Npgsql.NpgsqlConnectionStringBuilder(containerConnection)
        {
            Database = "EstimatorX"
        };
        var connectionString = connectionBuilder.ToString();

        // override connection string to use docker container
        var configurationData = new Dictionary<string, string?>
        {
            ["Database:Provider"] = "PostgreSQL",
            ["Database:ConnectionString"] = connectionString
        };
        builder.Configuration.AddInMemoryCollection(configurationData);

        // Register the memory email delivery service for testing purposes
        services
            .AddEmailServices(options =>
            {
                options.FromAddress = "noreply@estimatorx.com";
                options.FromName = "EstimatorX Test";
            })
            .AddEmailDelivery<MemoryEmailDeliverService>();

        services.TryAddSingleton<IBaseAddressResolver, MockBaseAddressResolver>();

        services
            .AddEstimatorXShared()
            .AddEstimatorXCore();

        // seed default test user after database migration
        services.AddHostedService<DatabaseSeeder>();
    }

    private sealed class TestBaseAddressResolver : IBaseAddressResolver
    {
        public string GetBaseAddress(string? path = null) => path is null ? "https://localhost" : $"https://localhost/{path.TrimStart('/')}";
    }
}
