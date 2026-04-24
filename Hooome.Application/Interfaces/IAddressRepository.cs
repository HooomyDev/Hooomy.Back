using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IAddressRepository : IRepository<Address>
{
    Task<Address?> GetByCoords(double lat, double lng, CancellationToken cancellationToken = default);
    Task<List<Address>> GetByQuery(string query, CancellationToken cancellationToken = default);
}
