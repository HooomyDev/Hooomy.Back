using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class AddressRepository(HooomeDbContext dbContext)
    : BaseRepository<Address>(dbContext), IAddressRepository
{
    public async Task AddAddress(Guid companyId, Guid addressId, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies
                .FirstOrDefaultAsync(c => c.Id == companyId, cancellationToken)
                ?? throw new NotFoundException(nameof(Company), companyId);

        var address = await _dbContext.Addresses
            .FirstOrDefaultAsync(a => a.Id == addressId, cancellationToken)
            ?? throw new NotFoundException(nameof(Address), addressId);

        address.ServicedByCompanyId = companyId;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

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
