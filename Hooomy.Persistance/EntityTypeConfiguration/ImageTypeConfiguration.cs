using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class ImageTypeConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(i => i.RequestId);

        builder.HasIndex(i => i.IsMain);
    }
}
