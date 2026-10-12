using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstimatorX.Data.Mapping;

public class DataProtectionKeyMap : IEntityTypeConfiguration<DataProtectionKey>
{
    public void Configure(EntityTypeBuilder<DataProtectionKey> builder)
    {
        // table
        builder.ToTable(Table.Name);

        // key
        builder.HasKey(t => t.Id)
            .HasName("PK_DataProtectionKey");

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName(Columns.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.FriendlyName)
            .HasColumnName(Columns.FriendlyName)
            .HasMaxLength(256);

        builder.Property(t => t.Xml)
            .HasColumnName(Columns.Xml);
    }

    public readonly struct Table
    {
        public const string Name = "DataProtectionKey";
    }

    public readonly struct Columns
    {
        public const string Id = "Id";
        public const string FriendlyName = "FriendlyName";
        public const string Xml = "Xml";
    }
}
