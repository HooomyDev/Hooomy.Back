using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IRequestImageRepository 
    : IRepository<RequestImage>
{
    Task<IEnumerable<RequestImage>> GetAllByRequestId(Guid requestId, CancellationToken cancellationToken = default);
}
