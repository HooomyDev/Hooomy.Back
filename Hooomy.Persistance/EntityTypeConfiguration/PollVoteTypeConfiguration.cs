using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class PollVoteTypeConfiguration : IEntityTypeConfiguration<PollVote>
{
    public void Configure(EntityTypeBuilder<PollVote> builder)
    {
        builder.HasKey(pv => pv.Id);
        
        builder.HasIndex(pv => pv.Id)
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
            .WithMany(p => p.Votes) 
            .HasForeignKey(pv => pv.PollId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(pv => pv.Option)
            .WithMany(o => o.Votes) 
            .HasForeignKey(pv => pv.OptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}