using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Interfaces;

public interface IPollRepository : IRepository<Poll>
{
    Task<(IEnumerable<Poll> Items, int TotalCount)> GetFilteredPolls(string? title = null,
        int page = 1,
        int pageSize = 10,
        PollType? type = PollType.Unknown, 
        PollStatus? status = PollStatus.Unknown, 
        Guid? companyId = null,
        CancellationToken cancellationToken = default);
}
