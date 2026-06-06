using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ResidentId)
            .IsRequired();

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(c => c.ResidentId)
            .HasDatabaseName("IX_Chats_ResidentId");

        builder.HasIndex(c => c.CompanyId)
            .HasDatabaseName("IX_Chats_CompanyId");

        builder.HasIndex(c => c.Status)
            .HasDatabaseName("IX_Chats_Status");

        builder.HasIndex(c => c.CreatedAt)
            .HasDatabaseName("IX_Chats_CreatedAt");

        builder.HasIndex(c => c.UpdatedAt)
            .HasDatabaseName("IX_Chats_UpdatedAt");
    }
}