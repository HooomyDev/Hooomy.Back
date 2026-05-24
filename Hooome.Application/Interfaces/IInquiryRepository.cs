using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IInquiryRepository : IRepository<Inquiry>
{
    Task<(ICollection<Inquiry> Items, int TotalCount)> GetAllWithPagination(
        int page = 1, 
        int pageSize = 10, 
        DateTime? date = null,
        CancellationToken cancellationToken = default);
}
