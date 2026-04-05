using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class FavoriteAddressTypeConfiguration
    : IEntityTypeConfiguration<FavoriteAddress>
{
    public void Configure(EntityTypeBuilder<FavoriteAddress> builder)
    {
        builder.HasKey(fa => fa.Id);
        builder.HasIndex(fa => fa.Id).IsUnique();
    }
}
