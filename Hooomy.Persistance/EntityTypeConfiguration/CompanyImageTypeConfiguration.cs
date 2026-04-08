using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class CompanyImageTypeConfiguration : IEntityTypeConfiguration<CompanyImage>
{
    public void Configure(EntityTypeBuilder<CompanyImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.CompanyId)
            .IsRequired();

        builder.HasOne(i => i.Company)
            .WithOne(c => c.Logo)
            .HasForeignKey<CompanyImage>(i => i.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.IsMain);
    }
}
