using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Interfaces;

public interface IRequestCommentRepository : IRepository<RequestComment>
{
    Task<(ICollection<RequestComment> items, int totalCount)> GetByRequestId(string? text, RequestCommentStatus? status, Guid? requestId, int page = 1, int pageSize = 5, CancellationToken cancellationToken = default);
    Task<int> CountWithFilter(Guid? requestId = null, string? filter = "all", CancellationToken cancellationToken = default);
}
