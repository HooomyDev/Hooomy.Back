using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class RequestImageTypeConfiguration : IEntityTypeConfiguration<RequestImage>
{
    public void Configure(EntityTypeBuilder<RequestImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.RequestId)
            .IsRequired();

        builder.HasOne(i => i.Request)
            .WithMany(r => r.Images)
            .HasForeignKey(i => i.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.IsMain);
    }
}