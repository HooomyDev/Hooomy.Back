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
    public DbSet<Street> Streets { get; set; }
    public DbSet<Work> Works { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new RequestTypeConfiguration());
        builder.ApplyConfiguration(new FavoriteAddressTypeConfiguration());
        builder.ApplyConfiguration(new StreetTypeConfiguration());
        builder.ApplyConfiguration(new WorkEntityTypeConfiguration());
        base.OnModelCreating(builder);
    }
}
