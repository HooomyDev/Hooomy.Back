using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IRequestReviewRepository : IRepository<RequestReview>
{
    Task<RequestReview?> GetByRequestId(Guid requestId, CancellationToken cancellationToken = default);
    Task<ICollection<RequestReview>?> GetByCompanyId(Guid requestId, CancellationToken cancellationToken = default);
}
