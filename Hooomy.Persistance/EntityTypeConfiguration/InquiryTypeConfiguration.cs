using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class InquiryTypeConfiguration : IEntityTypeConfiguration<Inquiry>
{
    public void Configure(EntityTypeBuilder<Inquiry> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Message)
            .IsRequired()
            .HasMaxLength(2000);
        
        builder.Property(i => i.UserEmail)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.HasIndex(i => i.CreatedAt);
    }
}