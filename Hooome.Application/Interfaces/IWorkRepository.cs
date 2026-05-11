using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IWorkRepository : IRepository<Work>
{
    Task<ICollection<Work>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
