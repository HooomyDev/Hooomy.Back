using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class RequestReviewTypeConfiguration : IEntityTypeConfiguration<RequestReview>
{
    public void Configure(EntityTypeBuilder<RequestReview> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.RequestId)
            .IsUnique(false);
        
        builder.HasIndex(r => r.UserId);
        
        builder.HasIndex(r => r.Score);

        builder.Property(r => r.Score)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(r => r.Text)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.RequestId)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(r => r.Request)
            .WithOne(r => r.Review)
            .HasForeignKey<RequestReview>(r => r.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}