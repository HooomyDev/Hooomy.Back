using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IAddressRepository : IRepository<Address>
{
    Task AddAddress(Guid companyId, Guid addressId, CancellationToken cancellationToken = default);
    Task RemoveAddress(Guid companyId, Guid addressId, CancellationToken cancellationToken = default);
    Task<Address?> GetByCoords(double lat, double lng, CancellationToken cancellationToken = default);
    Task<List<Address>> GetByQuery(string query, CancellationToken cancellationToken = default);
}
