using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class PollOptionTypeConfiguration : IEntityTypeConfiguration<PollOption>
{
    public void Configure(EntityTypeBuilder<PollOption> builder)
    {
        builder.HasKey(po => po.Id);
        builder.HasIndex(po => po.Id).IsUnique();

        builder.Property(po => po.PollId)
            .IsRequired();

        builder.Property(po => po.Content)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(po => po.CreatedAt)
            .IsRequired();

        builder.HasOne<Poll>()
            .WithMany()
            .HasForeignKey(po => po.PollId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
