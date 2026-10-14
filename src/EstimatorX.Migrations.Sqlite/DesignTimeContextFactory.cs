using EstimatorX.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EstimatorX.Migrations.Sqlite;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<EstimatorContext>
{
    public EstimatorContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ESTIMATORX_SQLITE_CONNECTION")
            ?? "Data Source=EstimatorX.db";

        var options = new DbContextOptionsBuilder<EstimatorContext>()
            .UseEstimatorProvider(DatabaseProvider.SQLite, connectionString)
            .Options;

        return new EstimatorContext(options);
    }
}
