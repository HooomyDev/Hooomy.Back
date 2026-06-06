using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class RequestCommentImageTypeConfiguration : IEntityTypeConfiguration<RequestCommentImage>
{
    public void Configure(EntityTypeBuilder<RequestCommentImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.OriginalFileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.CommentId)
            .IsRequired();

        builder.Property(i => i.ContentType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(i => i.UploadedAt)
            .IsRequired();

        builder.HasOne(i => i.Comment)
            .WithMany(r => r.Images)
            .HasForeignKey(i => i.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
