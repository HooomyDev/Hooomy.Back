using Hooome.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hooome.Application.Chats.Queries.GetChatList;

public class GetChatListQueryHandler(IHooomeDbContext dbContext)
    : IRequestHandler<GetChatListQuery, ChatListVm>
{
    public async Task<ChatListVm> Handle(GetChatListQuery request, CancellationToken cancellationToken)
    {
        //get all chats for user
        var chatsQuery = dbContext.Chats
            .Where(x => x.ResidentId == request.UserId);

        //map to ChatListLookupDto with sub-queries for last message data
        var chats = await chatsQuery.Select(x => new ChatListLookupDto
        {
            Id = x.Id,
            CompanyName = x.Company.Name,
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

        return new ChatListVm { Chats = chats };
    }
}
