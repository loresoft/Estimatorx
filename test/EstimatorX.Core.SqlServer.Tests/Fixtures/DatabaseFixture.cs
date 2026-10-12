using Arbiter.CommandQuery.Definitions;
using Arbiter.Communication;
using Arbiter.Communication.Email;

using EstimatorX.Core.Tests;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Testcontainers.MsSql;

using XUnit.Hosting.Logging;

namespace EstimatorX.Core.SqlServer.Tests.Fixtures;

public class DatabaseFixture : TestApplicationFixture, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2019-latest")
        .WithPassword("Bn87bBYhLjYRj%9zRgUc")
        .Build();

    public async ValueTask InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    protected override void ConfigureApplication(HostApplicationBuilder builder)
    {
        base.ConfigureApplication(builder);

        builder.Logging.AddMemoryLogger();

        var services = builder.Services;

        // change database from container default
        string containerConnection = _msSqlContainer.GetConnectionString();
        var connectionBuilder = new SqlConnectionStringBuilder(containerConnection)
        {
            InitialCatalog = "EstimatorX"
        };
        var connectionString = connectionBuilder.ToString();

        connectionString = "Data Source=(local);Initial Catalog=EstimatorX;Integrated Security=True;TrustServerCertificate=True;";

        // override connection string to use docker container
        var configurationData = new Dictionary<string, string?>
        {
            ["Database:Provider"] = "SQLServer",
            ["Database:ConnectionString"] = connectionString,
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
    }

    private sealed class TestBaseAddressResolver : IBaseAddressResolver
    {
        public string GetBaseAddress(string? path = null) => path is null ? "https://localhost" : $"https://localhost/{path.TrimStart('/')}";
    }
}
