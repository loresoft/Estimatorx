using Arbiter.CommandQuery.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstimatorX.Data.Mapping;

public class ProjectMap : IEntityTypeConfiguration<Entities.Project>
{
    public void Configure(EntityTypeBuilder<Entities.Project> builder)
    {
        // table
        builder.ToTable(Table.Name);

        // key
        builder.HasKey(t => t.Id)
            .IsClustered()
            .HasName("PK_Project");

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName(Columns.Id)
            .UseSnowflake();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasColumnName(Columns.Name)
            .HasMaxLength(255);

        builder.Property(t => t.Description)
            .HasColumnName(Columns.Description)
            .HasMaxLength(2000);

        builder.Property(t => t.OrganizationId)
            .HasColumnName(Columns.OrganizationId);

        builder.Property(t => t.UserId)
            .HasColumnName(Columns.UserId);

        builder.ComplexProperty(
            propertyExpression: t => t.Settings,
            buildAction: b => b.ToJson(Columns.Settings)
        );

        builder.ComplexCollection(
            propertyExpression: t => t.Epics,
            buildAction: b => b.ToJson(Columns.Epics)
        );

        builder.Property(t => t.EstimatedTotal)
            .HasColumnName(Columns.EstimatedTotal);

        builder.Property(t => t.WeightedTotal)
            .HasColumnName(Columns.WeightedTotal);

        builder.Property(t => t.EstimatedWeeks)
            .HasColumnName(Columns.EstimatedWeeks);

        builder.Property(t => t.WeightedWeeks)
            .HasColumnName(Columns.WeightedWeeks);

        builder.Property(t => t.EstimatedCost)
            .HasColumnName(Columns.EstimatedCost)
            .HasPrecision(18, 2);

        builder.Property(t => t.WeightedCost)
            .HasColumnName(Columns.WeightedCost)
            .HasPrecision(18, 2);

        builder.Property(t => t.SecurityKey)
            .HasColumnName(Columns.SecurityKey)
            .HasMaxLength(64);

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
        builder.HasOne(t => t.Organization)
            .WithMany(t => t.Projects)
            .HasForeignKey(d => d.OrganizationId)
            .HasConstraintName("FK_Project_Organization_OrganizationId");

        builder.HasOne(t => t.User)
            .WithMany(t => t.Projects)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_Project_User_UserId");

        // indexes
        builder.HasIndex(t => t.OrganizationId)
            .HasDatabaseName("IX_Project_OrganizationId");

        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("IX_Project_UserId");

        builder.HasIndex(t => t.SecurityKey)
            .IsUnique()
            .HasDatabaseName("UX_Project_SecurityKey");
    }

    public readonly struct Table
    {
        public const string Name = "Project";
    }

    public readonly struct Columns
    {
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Description = "Description";
        public const string OrganizationId = "OrganizationId";
        public const string UserId = "UserId";
        public const string Settings = "Settings";
        public const string Epics = "Epics";
        public const string EstimatedTotal = "EstimatedTotal";
        public const string WeightedTotal = "WeightedTotal";
        public const string EstimatedWeeks = "EstimatedWeeks";
        public const string WeightedWeeks = "WeightedWeeks";
        public const string EstimatedCost = "EstimatedCost";
        public const string WeightedCost = "WeightedCost";
        public const string SecurityKey = "SecurityKey";
        public const string Created = "Created";
        public const string CreatedBy = "CreatedBy";
        public const string Updated = "Updated";
        public const string UpdatedBy = "UpdatedBy";
    }
}
