using Hooome.Domain;
using Hooome.Domain.Enums;

namespace Hooome.Application.Interfaces;

public interface IComplaintRepository : IRepository<Complaint>
{
    Task<List<Complaint>> GetAllWithFilters(ComplaintStatus status,
        ComplaintType type,
        string shortDescription,
        CancellationToken cancellationToken = default);
}
