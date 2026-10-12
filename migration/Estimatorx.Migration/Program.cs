
using Estimatorx.Migration.Commands;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Spectre.Console.Cli;

namespace Estimatorx.Migration;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            var builder = Host.CreateApplicationBuilder(args);

            // Load user secrets in local environments
            builder.Configuration.AddUserSecrets(typeof(Program).Assembly, true);

            ConfigureServices(builder);

            var host = builder.Build();
            var app = ConfigureCommands(host);

            return await app.RunAsync(args);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Host terminated unexpectedly: {ex.Message}");
            Console.Error.WriteLine(ex.ToString());
            return 1;
        }
    }

    private static void ConfigureServices(HostApplicationBuilder builder)
    {
        builder.Services
            .AddCosmosRepository();

        builder.Services
            .AddEstimatorXShared()
            .AddEstimatorXCore()
            .AddEstimatorxDataCosmos()
            .AddEstimatorxMigration();
    }

    private static CommandApp ConfigureCommands(IHost host)
    {
        // Create a type registrar that uses our DI container
        var registrar = new TypeRegistrar(host.Services);
        var app = new CommandApp(registrar);

        app.Configure(config =>
        {
            config.SetApplicationName("Migration");

            config.AddCommand<PopulateKeysCommand>("populate-keys")
                .WithDescription("Populate missing Key values using Snowflake ids");

            config.AddCommand<MigrateDataCommand>("migrate-data")
                .WithDescription("Migrate Cosmos entities into the EstimatorX database");
        });

        return app;
    }

}
