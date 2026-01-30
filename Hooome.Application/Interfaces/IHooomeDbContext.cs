using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.Interfaces;

public interface IHooomeDbContext
{
    DbSet<Request> Requests { get; set; }
    DbSet<FavoriteAddress> FavoriteAddresses { get; set; }
    DbSet<Street> Streets { get; set; }
    DbSet<Work> Works { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
