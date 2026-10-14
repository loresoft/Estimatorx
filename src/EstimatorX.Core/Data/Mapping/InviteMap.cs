using Arbiter.CommandQuery.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstimatorX.Data.Mapping;

public class InviteMap : IEntityTypeConfiguration<Entities.Invite>
{
    public void Configure(EntityTypeBuilder<Entities.Invite> builder)
    {
        // table
        builder.ToTable(Table.Name);

        // key
        builder.HasKey(t => t.Id)
            .IsClustered()
            .HasName("PK_Invite");

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName(Columns.Id)
            .UseSnowflake();

        builder.Property(t => t.Email)
            .IsRequired()
            .HasColumnName(Columns.Email)
            .HasMaxLength(256);

        builder.Property(t => t.Name)
            .HasColumnName(Columns.Name)
            .HasMaxLength(256);

        builder.Property(t => t.OrganizationId)
            .IsRequired()
            .HasColumnName(Columns.OrganizationId);

        builder.Property(t => t.SecurityCode)
            .HasColumnName(Columns.SecurityCode)
            .HasMaxLength(64);

        builder.Property(t => t.Expiration)
            .IsRequired()
            .HasColumnName(Columns.Expiration);

        builder.Property(t => t.LastSent)
            .HasColumnName(Columns.LastSent);

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
            .WithMany(t => t.Invites)
            .HasForeignKey(d => d.OrganizationId)
            .HasConstraintName("FK_Invite_Organization_OrganizationId");


        // indexes
        builder.HasIndex(t => new { t.OrganizationId, t.Email })
            .IsUnique()
            .HasDatabaseName("UX_Invite_OrganizationId_Email");

    }

    public readonly struct Table
    {
        public const string Name = "Invite";
    }

    public readonly struct Columns
    {
        public const string Id = "Id";
        public const string Email = "Email";
        public const string Name = "Name";
        public const string OrganizationId = "OrganizationId";
        public const string SecurityCode = "SecurityKey";
        public const string Expiration = "Expiration";
        public const string LastSent = "LastSent";
        public const string Created = "Created";
        public const string CreatedBy = "CreatedBy";
        public const string Updated = "Updated";
        public const string UpdatedBy = "UpdatedBy";
    }
}
