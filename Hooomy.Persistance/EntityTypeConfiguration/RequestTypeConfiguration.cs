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
        builder.Property(r => r.Title).IsRequired().HasMaxLength(150);
        builder.Property(r => r.Description).HasMaxLength(300);
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.Category).IsRequired();

        builder.Property(r => r.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(r => r.DeletedAt)
            .IsRequired(false);

        builder.HasMany(r => r.Images)
            .WithOne(i => i.Request)
            .HasForeignKey(i => i.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Comments)
            .WithOne(i => i.Request)
            .HasForeignKey(i => i.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Address)
            .WithMany(a => a.Requests)
            .HasForeignKey(r => r.AddressId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(r => r.Notifications)
            .WithOne(i => i.Request)
            .HasForeignKey(rn => rn.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
