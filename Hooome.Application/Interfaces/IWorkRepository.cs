using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Interfaces;

public interface IWorkRepository : IRepository<Work>
{
    Task<ICollection<Work>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<(ICollection<Work> Items, int TotalCount)> GetAllWithPagination(
        int page = 1,
        int pageSize = 10,
        RequestCategory? category = null,
        WorkSeriousness? seriousness = null,
        Guid? addressId = null,
        string? searchTitle = null,
        Guid? companyId = null,
        CancellationToken cancellationToken = default);
}
