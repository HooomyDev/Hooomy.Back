using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class WorkEntityTypeConfiguration : IEntityTypeConfiguration<Work>
{
    public void Configure(EntityTypeBuilder<Work> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(w => w.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(w => w.Category).IsRequired();
        builder.Property(w => w.Seriousness).IsRequired();
        builder.Property(w => w.PlannedStartTime).IsRequired();
        builder.Property(w => w.PlannedEndTime).IsRequired();
        builder.Property(w => w.FactStartTime);
        builder.Property(w => w.FactEndTime);

        builder.HasMany(w => w.Notifications)
            .WithOne(wn => wn.Work)
            .HasForeignKey(wn => wn.WorkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
