using Hooome.Application.Common.Exceptions;
using Hooome.Application.Interfaces;
using Hooome.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.CQRS.Chats.Queries.GetChatDetailsQuery;

public class GetChatDetailsQueryHandler(IHooomeDbContext dbContext)
    : IRequestHandler<GetChatDetailsQuery, ChatDetailsVm>
{
    public async Task<ChatDetailsVm> Handle(GetChatDetailsQuery request, CancellationToken cancellationToken)
    {
        var chatVm = await dbContext.Chats
            .Where(x => x.Id == request.ChatId)
            .Select(x => new ChatDetailsVm
            {
                Id = x.Id,
                CompanyName = x.Company.Name,
                ResidentName = x.ResidentName,
                Status = x.Status,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Messages = x.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(50)
                    .OrderBy(m => m.CreatedAt)
                    .ToList(),
                LastMessageSentAt = x.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.CreatedAt)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(cancellationToken) 
                ?? throw new NotFoundException(nameof(Chat), 
                    $"ResidentId: {request.ResidentId}, ChatId: {request.ChatId}");

        return chatVm;
    }
}
