using Hooome.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hooome.Persistance.EntityTypeConfiguration;

public class CompanyTypeConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Id).IsUnique();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.Property(c => c.WorkingHours)
            .HasMaxLength(100);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasMany(c => c.Polls)
            .WithOne(p => p.Company)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Address)
            .WithOne(a => a.RegisteredCompany)
            .HasForeignKey<Address>(a => a.RegisteredCompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.ServedAddresses)
            .WithOne(a => a.ServicedByCompany)
            .HasForeignKey(a => a.ServicedByCompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.Logo)
            .WithOne(i => i.Company)
            .HasForeignKey<CompanyImage>(i => i.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
