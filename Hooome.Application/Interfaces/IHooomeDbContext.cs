using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.Interfaces;

public interface IHooomeDbContext
{
    DbSet<Request> Requests { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
