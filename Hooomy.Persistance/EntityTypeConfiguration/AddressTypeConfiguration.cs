using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class AddressTypeConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Street)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.HouseNumber)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(a => new { a.Street, a.HouseNumber });
        builder.HasIndex(a => new { a.Latitude, a.Longitude });

        builder.HasMany(a => a.FavoriteAddresses)
            .WithOne(fa => fa.Address)
            .HasForeignKey(a => a.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Works)
            .WithOne(w => w.Address)
            .HasForeignKey(a => a.AddressId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.ServicedByCompany)
            .WithMany(c => c.ServedAddresses)
            .HasForeignKey(a => a.ServicedByCompanyId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
