using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Persistance.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance;

public sealed class HooomeDbContext(DbContextOptions<HooomeDbContext> options) 
    : DbContext(options), IHooomeDbContext
{
    public DbSet<Request> Requests { get; set; }
    public DbSet<FavoriteAddress> FavoriteAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new RequestTypeConfiguration());
        builder.ApplyConfiguration(new FavoriteAddressTypeConfiguration());
        base.OnModelCreating(builder);
    }
}
