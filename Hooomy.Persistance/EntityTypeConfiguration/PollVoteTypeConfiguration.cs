using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class PollVoteTypeConfiguration : IEntityTypeConfiguration<PollVote>
{
    public void Configure(EntityTypeBuilder<PollVote> builder)
    {
        builder.HasKey(pv => pv.Id);
        builder.HasIndex(pv => pv.Id).IsUnique();

        builder.HasIndex(pv => new { pv.PollId, pv.UserId })
            .IsUnique();

        builder.Property(pv => pv.PollId)
            .IsRequired();

        builder.Property(pv => pv.OptionId)
            .IsRequired();

        builder.Property(pv => pv.UserId)
            .IsRequired();

        builder.Property(pv => pv.CreatedAt)
            .IsRequired();

        builder.HasOne(pv => pv.Poll)
            .WithMany()
            .HasForeignKey(pv => pv.PollId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pv => pv.Option)
            .WithMany()
            .HasForeignKey(pv => pv.OptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}