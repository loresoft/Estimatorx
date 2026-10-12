using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EstimatorX.Data;

public class EstimatorContext(DbContextOptions<EstimatorContext> options) : DbContext(options)
{
    public virtual DbSet<Entities.Invite> Invites { get; set; } = null!;

    public virtual DbSet<Entities.Organization> Organizations { get; set; } = null!;

    public virtual DbSet<Entities.Project> Projects { get; set; } = null!;

    public virtual DbSet<Entities.Template> Templates { get; set; } = null!;

    public virtual DbSet<Entities.User> Users { get; set; } = null!;

    public virtual DbSet<Entities.UserLogin> UserLogins { get; set; } = null!;

    public virtual DbSet<Entities.UserOrganization> UserOrganizations { get; set; } = null!;

    public virtual DbSet<Entities.UserProvider> UserProviders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Mapping.InviteMap());
        modelBuilder.ApplyConfiguration(new Mapping.OrganizationMap());
        modelBuilder.ApplyConfiguration(new Mapping.ProjectMap());
        modelBuilder.ApplyConfiguration(new Mapping.TemplateMap());
        modelBuilder.ApplyConfiguration(new Mapping.UserLoginMap());
        modelBuilder.ApplyConfiguration(new Mapping.UserMap());
        modelBuilder.ApplyConfiguration(new Mapping.UserOrganizationMap());
        modelBuilder.ApplyConfiguration(new Mapping.UserProviderMap());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // SQLite cannot order or compare DateTimeOffset values; store them as binary (long)
        if (Database.IsSqlite())
        {
            configurationBuilder.Properties<DateTimeOffset>().HaveConversion<DateTimeOffsetToBinaryConverter>();
            configurationBuilder.Properties<DateTimeOffset?>().HaveConversion<DateTimeOffsetToBinaryConverter>();
        }
    }
}
