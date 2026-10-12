using EstimatorX.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EstimatorX.Migrations.PostgreSql;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<EstimatorContext>
{
    public EstimatorContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ESTIMATORX_POSTGRESQL_CONNECTION")
            ?? "Host=localhost;Database=EstimatorX;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<EstimatorContext>()
            .UseEstimatorProvider(DatabaseProvider.PostgreSQL, connectionString)
            .Options;

        return new EstimatorContext(options);
    }
}
