using Arbiter.CommandQuery.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstimatorX.Data.Mapping;

public class UserMap : IEntityTypeConfiguration<Entities.User>
{
    public void Configure(EntityTypeBuilder<Entities.User> builder)
    {
        // table
        builder.ToTable(Table.Name);

        // key
        builder.HasKey(t => t.Id)
            .IsClustered()
            .HasName("PK_User");

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName(Columns.Id)
            .UseSnowflake();

        builder.Property(t => t.Email)
            .HasColumnName(Columns.Email)
            .HasMaxLength(256);

        builder.Property(t => t.Name)
            .HasColumnName(Columns.Name)
            .HasMaxLength(256);

        builder.Property(t => t.IsAdministrator)
            .IsRequired()
            .HasColumnName(Columns.IsAdministrator)
            .HasDefaultValue(false, "DF_User_IsAdministrator");

        builder.Property(t => t.IsDisabled)
            .IsRequired()
            .HasColumnName(Columns.IsDisabled)
            .HasDefaultValue(false, "DF_User_IsDisabled");

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

        // indexes
        builder.HasIndex(t => t.Email)
            .IsUnique()
            .HasDatabaseName("UX_User_Email");

    }

    public readonly struct Table
    {
        public const string Name = "User";
    }

    public readonly struct Columns
    {
        public const string Id = "Id";
        public const string Email = "Email";
        public const string Name = "Name";
        public const string IsAdministrator = "IsAdministrator";
        public const string IsDisabled = "IsDisabled";
        public const string Created = "Created";
        public const string CreatedBy = "CreatedBy";
        public const string Updated = "Updated";
        public const string UpdatedBy = "UpdatedBy";
    }
}
