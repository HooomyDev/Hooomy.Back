using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class AddressRepository(HooomeDbContext dbContext)
    : BaseRepository<Address>(dbContext), IAddressRepository
{
    public async Task<Address?> GetByCoords(double lat, double lng, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(a =>
                 a.Latitude == (decimal)lat &&
                 a.Longitude == (decimal)lng, cancellationToken);

    public async Task<List<Address>> GetByQuery(string query, CancellationToken cancellationToken = default) 
        => await _dbSet.Where(a => EF.Functions.ILike(a.Street, $"%{query}%"))
            .OrderBy(s => s.Street)
            .Take(15)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}
