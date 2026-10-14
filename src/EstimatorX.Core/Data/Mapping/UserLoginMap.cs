using Arbiter.CommandQuery.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstimatorX.Data.Mapping;

public class UserLoginMap : IEntityTypeConfiguration<Entities.UserLogin>
{
    public void Configure(EntityTypeBuilder<Entities.UserLogin> builder)
    {
        // table
        builder.ToTable(Table.Name);

        // key
        builder.HasKey(t => t.Id)
            .IsClustered()
            .HasName("PK_UserLogin");

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName(Columns.Id)
            .UseSnowflake();

        builder.Property(t => t.UserId)
            .IsRequired()
            .HasColumnName(Columns.UserId);

        builder.Property(t => t.IpAddress)
            .HasColumnName(Columns.IpAddress)
            .HasMaxLength(50);

        builder.Property(t => t.UserAgent)
            .HasColumnName(Columns.UserAgent);

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
            .WithMany(t => t.UserLogins)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserLogin_User_UserId");

        // indexes
        builder.HasIndex(t => t.UserId)
            .HasDatabaseName("IX_UserLogin_UserId");
    }

    public readonly struct Table
    {
        public const string Name = "UserLogin";
    }

    public readonly struct Columns
    {
        public const string Id = "Id";
        public const string UserId = "UserId";
        public const string IpAddress = "IpAddress";
        public const string UserAgent = "UserAgent";
        public const string Created = "Created";
        public const string CreatedBy = "CreatedBy";
        public const string Updated = "Updated";
        public const string UpdatedBy = "UpdatedBy";
    }
}
