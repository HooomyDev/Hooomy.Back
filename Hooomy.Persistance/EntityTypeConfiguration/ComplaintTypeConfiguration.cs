using Hooome.Domain;
using Hooome.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class ComplaintTypeConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.ShortDescription)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasDefaultValue(ComplaintType.Unknown);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(ComplaintStatus.Unknown);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.HasOne(c => c.Request)
            .WithMany() 
            .HasForeignKey(c => c.RequestId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => c.UserId);

        builder.HasIndex(c => c.Type);

        builder.HasIndex(c => c.Status);

        builder.HasIndex(c => c.CreatedAt);

        builder.HasIndex(c => new { c.Type, c.Status });

        builder.HasIndex(c => new { c.UserId, c.Status });
    }
}