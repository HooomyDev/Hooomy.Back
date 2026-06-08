using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class RequestNotificationTypeConfiguration : IEntityTypeConfiguration<RequestNotification>
{
    public void Configure(EntityTypeBuilder<RequestNotification> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}