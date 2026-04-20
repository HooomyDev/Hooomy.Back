using Hooome.Domain;

namespace Hooome.Application.Interfaces;

public interface IChatRepository : IRepository<Chat>
{
    Task<bool> IsChatExist(Guid companyId, Guid residentId, 
        CancellationToken cancellationToken = default);

    Task<List<Chat>> GetAllByResidentId(Guid residentId,
        CancellationToken cancellationToken = default);
    
    Task<List<Chat>> GetAllByCompanyId(Guid companyId,
        CancellationToken cancellationToken = default);
}