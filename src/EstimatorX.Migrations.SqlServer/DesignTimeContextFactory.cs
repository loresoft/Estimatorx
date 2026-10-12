using EstimatorX.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EstimatorX.Migrations.SqlServer;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<EstimatorContext>
{
    public EstimatorContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ESTIMATORX_SQLSERVER_CONNECTION")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=EstimatorX;Trusted_Connection=True;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<EstimatorContext>()
            .UseEstimatorProvider(DatabaseProvider.SqlServer, connectionString)
            .Options;

        return new EstimatorContext(options);
    }
}
