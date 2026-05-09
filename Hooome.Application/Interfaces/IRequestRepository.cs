using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Interfaces;

public interface IRequestRepository : IRepository<Request>
{
    Task<Request?> GetByIdAndUserId(Guid requestId, 
        Guid userId, 
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Request>> GetFilteredRequests(
        Guid userId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        RequestStatus? status = null,
        CancellationToken cancellationToken = default);

    Task<Dictionary<string, int>> GetRequestsByDate(
        DateTime startDate,
        DateTime endDate,
        Guid? companyId,
        CancellationToken cancellationToken = default);

    Task<(IEnumerable<Request> Items, int TotalCount)> GetRequestsWithPagination(
        string? title = null,
        Guid? companyId = null,
        RequestStatus? status = null,
        RequestCategory? category = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}

public enum StatisticGroupType
{
    Day,
    Month,
    Year
}