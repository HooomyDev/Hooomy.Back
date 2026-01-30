using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class StreetTypeConfiguration : IEntityTypeConfiguration<Street>
{
    public void Configure(EntityTypeBuilder<Street> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.Id).IsUnique();
        builder.Property(s => s.Title).IsRequired().HasMaxLength(100);
    }
}
