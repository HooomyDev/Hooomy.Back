using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class SystemNotificationTypeConfiguration : IEntityTypeConfiguration<SystemNotification>
{
    public void Configure(EntityTypeBuilder<SystemNotification> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}