using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class MessageTypeConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.ChatId)
            .IsRequired();

        builder.Property(m => m.SenderName)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(m => m.SenderType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(m => m.SenderId)
            .IsRequired();

        builder.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(m => m.MessageType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(m => m.IsRead)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.ReadAt)
            .IsRequired(false);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.HasOne(m => m.Chat)
            .WithMany(m => m.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.ChatId)
            .HasDatabaseName("IX_Messages_ChatId");

        builder.HasIndex(m => m.SenderId)
            .HasDatabaseName("IX_Messages_SenderId");

        builder.HasIndex(m => m.SenderType)
            .HasDatabaseName("IX_Messages_SenderType");

        builder.HasIndex(m => new { m.ChatId, m.CreatedAt })
            .HasDatabaseName("IX_Messages_Chat_Created");

        builder.HasIndex(m => new { m.ChatId, m.IsRead, m.SenderType })
            .HasDatabaseName("IX_Messages_Chat_Unread");

        builder.HasIndex(m => m.CreatedAt)
            .HasDatabaseName("IX_Messages_CreatedAt");

        builder.HasIndex(m => m.MessageType)
            .HasDatabaseName("IX_Messages_MessageType");
    }
}