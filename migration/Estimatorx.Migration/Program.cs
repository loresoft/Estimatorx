using Estimatorx.Data.Mongo;
using Estimatorx.Data.Mongo.Mapping;
using Estimatorx.Data.Mongo.Security;
using Estimatorx.Migration;

using EstimatorX.Core.Options;
using EstimatorX.Shared;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MongoDB.Abstracts;
using MongoDB.Bson.Serialization;

BsonClassMap.RegisterClassMap<ProjectMap>();
BsonClassMap.RegisterClassMap<TemplateMap>();
BsonClassMap.RegisterClassMap<OrganizationMap>();

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddAutoMapper(typeof(HostingConfiguration).Assembly, typeof(AssemblyMetadata).Assembly);

        services.AddHostedService<ImportHostedService>();

        services.AddCosmosRepository();

        services.AddSingleton(s =>
        {
            var configuration = s.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Estimatorx");

            return MongoFactory.GetDatabaseFromConnectionString(connectionString);
        });

        services.AddSingleton<ProjectRepository>();
        services.AddSingleton<TemplateRepository>();
        services.AddSingleton<OrganizationRepository>();

    })
    .RunConsoleAsync();
