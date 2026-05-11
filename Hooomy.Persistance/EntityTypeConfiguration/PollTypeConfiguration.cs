using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class PollTypeConfiguration : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Id).IsUnique();

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.CreatedBy)
            .IsRequired();

        builder.Property(p => p.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.Type)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.CompanyId)
            .IsRequired();

        builder.HasMany(p => p.Options)
           .WithOne(po => po.Poll)
           .HasForeignKey(po => po.PollId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Votes)
             .WithOne(pv => pv.Poll)
             .HasForeignKey(pv => pv.PollId)
             .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Company)
            .WithMany(c => c.Polls)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
