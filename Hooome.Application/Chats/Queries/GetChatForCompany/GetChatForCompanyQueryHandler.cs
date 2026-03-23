using Hooome.Application.Chats.Queries.GetChatList;
using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.Chats.Queries.GetChatForCompany;

public class GetChatForCompanyQueryHandler(IHooomeDbContext dbContext)
    : IRequestHandler<GetChatForCompanyQuery, ChatListForCompanyVm>
{
    public async Task<ChatListForCompanyVm> Handle(GetChatForCompanyQuery request, CancellationToken cancellationToken)
    {
        var chats = await dbContext.Chats
            .Where(x => x.CompanyId == request.CompanyId)
            .Select(x => new ChatListLookupDto
            {
                Id = x.Id,
                CompanyName = x.ResidentName,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                LastMessageContent = x.Messages
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => m.Content)
                .FirstOrDefault()
                    ?? "Сообщений пока нет, напишите первым!",
                LastMessageSentAt = x.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.CreatedAt)
                    .FirstOrDefault(),
                UnreadCount = x.Messages.Where(x => !x.IsRead).Count(),
            })
            .OrderByDescending(x => x.LastMessageSentAt ?? x.UpdatedAt)
            .ToListAsync(cancellationToken);

        return new ChatListForCompanyVm { Chats =  chats };
    }
}
