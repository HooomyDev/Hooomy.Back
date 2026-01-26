using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class RequestTypeConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.Id).IsUnique();
        builder.Property(r => r.Address).IsRequired().HasMaxLength(250);
        builder.Property(r => r.Description).HasMaxLength(300);
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.Category).IsRequired();
    }
}
