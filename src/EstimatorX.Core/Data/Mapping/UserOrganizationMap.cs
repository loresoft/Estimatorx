using Arbiter.CommandQuery.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstimatorX.Data.Mapping;

public class UserOrganizationMap : IEntityTypeConfiguration<Entities.UserOrganization>
{
    public void Configure(EntityTypeBuilder<Entities.UserOrganization> builder)
    {
        // table
        builder.ToTable(Table.Name);

        // key
        builder.HasKey(t => t.Id)
            .IsClustered()
            .HasName("PK_UserOrganization");

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName(Columns.Id)
            .UseSnowflake();

        builder.Property(t => t.UserId)
            .IsRequired()
            .HasColumnName(Columns.UserId);

        builder.Property(t => t.OrganizationId)
            .IsRequired()
            .HasColumnName(Columns.OrganizationId);

        builder.Property(t => t.IsOwner)
            .IsRequired()
            .HasColumnName(Columns.IsOwner)
            .HasDefaultValue(false, "DF_User_IsOwner");
        ;

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
            .WithMany(t => t.UserOrganizations)
            .HasForeignKey(d => d.OrganizationId)
            .HasConstraintName("FK_UserOrganization_Organization_OrganizationId");

        builder.HasOne(t => t.User)
            .WithMany(t => t.UserOrganizations)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_UserOrganization_User_UserId");

        // indexes
        builder.HasIndex(t => new { t.UserId, t.OrganizationId })
            .IsUnique()
            .HasDatabaseName("UX_UserOrganization_UserId_OrganizationId");

        builder.HasIndex(t => t.OrganizationId)
            .HasDatabaseName("IX_UserOrganization_OrganizationId");
    }

    public readonly struct Table
    {
        public const string Name = "UserOrganization";
    }

    public readonly struct Columns
    {
        public const string Id = "Id";
        public const string UserId = "UserId";
        public const string OrganizationId = "OrganizationId";
        public const string IsOwner = "IsOwner";
        public const string Created = "Created";
        public const string CreatedBy = "CreatedBy";
        public const string Updated = "Updated";
        public const string UpdatedBy = "UpdatedBy";
    }
}
