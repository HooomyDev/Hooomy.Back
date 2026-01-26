using Hooome.Application.Interfaces;
using Hooome.Domain;
using Hooome.Persistance.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hooome.Persistance;

public sealed class HooomeDbContext : DbContext, IHooomeDbContext
{
    public DbSet<Request> Requests { get; set; }

    public HooomeDbContext(DbContextOptions<HooomeDbContext> options)
        : base(options){ }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new RequestTypeConfiguration());
        base.OnModelCreating(builder);
    }
}
