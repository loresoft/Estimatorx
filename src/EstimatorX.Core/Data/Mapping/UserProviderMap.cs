using Arbiter.CommandQuery.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstimatorX.Data.Mapping;

public class UserProviderMap : IEntityTypeConfiguration<Entities.UserProvider>
{
    public void Configure(EntityTypeBuilder<Entities.UserProvider> builder)
    {
        // table
        builder.ToTable(Table.Name);

        // key
        builder.HasKey(t => t.Id)
            .IsClustered()
            .HasName("PK_UserProvider");

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName(Columns.Id)
            .UseSnowflake();

        builder.Property(t => t.Provider)
            .IsRequired()
            .HasColumnName(Columns.Provider)
            .HasMaxLength(100);

        builder.Property(t => t.ProviderKey)
            .IsRequired()
            .HasColumnName(Columns.ProviderKey)
            .HasMaxLength(450);

        builder.Property(t => t.UserId)
            .IsRequired()
            .HasColumnName(Columns.UserId);

        builder.Property(t => t.Created)
            .IsRequired()
            .HasColumnName(Columns.Created);

        builder.Property(t => t.CreatedBy)
            .HasColumnName(Columns.CreatedBy)
            .HasMaxLength(100);

        builder.Property(t => t.Updated)
            .IsRequired()
            .HasColumnName(Columns.Updated);

        builder.Property(t => t.UpdatedBy)
            .HasColumnName(Columns.UpdatedBy)
            .HasMaxLength(100);

        // relationships
        builder.HasOne(t => t.User)
            .WithMany(t => t.UserProviders)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_UserProvider_User_UserId");

        // indexes
        builder.HasIndex(t => new { t.Provider, t.ProviderKey })
            .IsUnique()
            .HasDatabaseName("UX_UserProvider_Provider_ProviderKey");

        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("IX_UserProvider_UserId");
    }

    public readonly struct Table
    {
        public const string Name = "UserProvider";
    }

    public readonly struct Columns
    {
        public const string Id = "Id";
        public const string Provider = "Provider";
        public const string ProviderKey = "ProviderKey";
        public const string UserId = "UserId";
        public const string Created = "Created";
        public const string CreatedBy = "CreatedBy";
        public const string Updated = "Updated";
        public const string UpdatedBy = "UpdatedBy";
    }
}
