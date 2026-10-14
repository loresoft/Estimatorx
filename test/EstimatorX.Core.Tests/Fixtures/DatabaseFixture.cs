using Arbiter.CommandQuery.Definitions;
using Arbiter.Communication;
using Arbiter.Communication.Email;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using XUnit.Hosting.Logging;

namespace EstimatorX.Core.Tests.Fixtures;

public class DatabaseFixture : TestApplicationFixture
{
    protected override void ConfigureApplication(HostApplicationBuilder builder)
    {
        base.ConfigureApplication(builder);

        builder.Logging.AddMemoryLogger();

        var services = builder.Services;

        var connectionString = $"Data Source=EstimatorX-{DateTime.Now.Ticks}.db";
        var configurationData = new Dictionary<string, string?>
        {
            ["Database:Provider"] = "SQLite",
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

        // seed default test user after database migration
        services.AddHostedService<DatabaseSeeder>();
    }
}
