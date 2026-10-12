using Arbiter.CommandQuery.EntityFramework;

using EstimatorX.Domain.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstimatorX.Data.Mapping;

public class OrganizationMap : IEntityTypeConfiguration<Entities.Organization>
{
    public void Configure(EntityTypeBuilder<Entities.Organization> builder)
    {
        // table
        builder.ToTable(Table.Name);

        // key
        builder.HasKey(t => t.Id)
            .IsClustered()
            .HasName("PK_Organization");

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName(Columns.Id)
            .UseSnowflake();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasColumnName(Columns.Name)
            .HasMaxLength(100);

        builder.Property(t => t.Description)
            .HasColumnName(Columns.Description)
            .HasMaxLength(255);

        builder.PrimitiveCollection(t => t.HostMatches)
            .HasColumnName(Columns.HostMatches)
            .ElementType(e => e.HasMaxLength(255));

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
        builder.HasIndex(t => t.Name)
            .IsUnique()
            .HasDatabaseName("UX_Organization_Name");

        // seed data
        builder.HasData(
            new Entities.Organization
            {
                Id = Organizations.Testing,
                Name = nameof(Organizations.Testing),
                Description = "Default testing organization",
                HostMatches = null,
                CreatedBy = "system",
                Created = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
                UpdatedBy = "system",
                Updated = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            }
        );
    }

    public readonly struct Table
    {
        public const string Name = "Organization";
    }

    public readonly struct Columns
    {
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Description = "Description";
        public const string HostMatches = "HostMatches";
        public const string Created = "Created";
        public const string CreatedBy = "CreatedBy";
        public const string Updated = "Updated";
        public const string UpdatedBy = "UpdatedBy";
    }
}
