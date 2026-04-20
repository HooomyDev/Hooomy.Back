using Hooome.Application.Interfaces;
using Hooome.Domain;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Persistance.Repositories;

public class ChatRepository(HooomeDbContext dbContext)
    : BaseRepository<Chat>(dbContext), IChatRepository
{
    public async override Task<Chat?> GetById(Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Company)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<Chat>> GetAllByCompanyId(Guid companyId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Company)
            .Include(c => c.Messages)
            .Where(c => c.CompanyId == companyId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Chat>> GetAllByResidentId(Guid residentId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Company)
            .Include(c => c.Messages)
            .Where(c => c.ResidentId == residentId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsChatExist(Guid companyId, Guid residentId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Chats
            .AnyAsync(c => c.CompanyId == companyId 
                && c.ResidentId == residentId, cancellationToken);
    }
}
