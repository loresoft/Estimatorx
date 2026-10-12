using Arbiter.Health;

using EstimatorX.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EstimatorX.Health;

public static class HealthRegistration
{
    [RegisterServices]
    public static void Register(IServiceCollection services)
    {
        services.AddHealthServices();

        var health = services.AddHealthChecks();

        // add EF database health check
        health.AddDbContextCheck<EstimatorContext>(
            name: "Database Connection",
            tags: ["Database"],
            customTestQuery: (context, token) => context.Organizations.AnyAsync(token)
        );
    }
}
