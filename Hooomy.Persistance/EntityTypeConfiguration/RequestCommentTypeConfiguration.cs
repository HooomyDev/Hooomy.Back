using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class RequestCommentTypeConfiguration : IEntityTypeConfiguration<RequestComment>
{
    public void Configure(EntityTypeBuilder<RequestComment> builder)
    {
        builder.HasKey(rc => rc.Id);
        builder.HasIndex(rc => rc.Id).IsUnique();

        builder.Property(rc => rc.RequestId)
            .IsRequired();

        builder.Property(rc => rc.UserId)
            .IsRequired();

        builder.Property(rc => rc.Text)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(rc => rc.PhotoUrl)
            .HasMaxLength(500);

        builder.Property(rc => rc.CreatedAt)
            .IsRequired();

        builder.HasOne(rc => rc.Request)
            .WithMany()
            .HasForeignKey(rc => rc.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
